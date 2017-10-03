# Stock synchronization application

## How to run the application
Checkout [repository](https://github.com/xrecoba/stock-sync-app) and execute:
```
stock-sync-app\stock-sync\Stock.Sync.App\dotnet run
```

* **Input** - This will use the contents of the file \stock-sync-app\stock-sync\Stock.Sync.App\inputFile.json, which by default contains the given example
* **Output** - The execution will create two files in the same folder as inputFile.json
  * outputFile.json - The expected output file (as in the given example).
  * errors.json - A list of the problems or errors found during execution (invalid syntax, non-existing product, etc...)

You can also open the project in any compatible IDE (like Visual Studio) and start (F5) the application.

To run the unit and integration tests, you should run this command:
```
stock-sync-app\stock-sync\Stock.Sync.Tests.Unit>dotnet test
```

> It would have been nice to gift-wrap the application inside a docker container so you could just run it with a single command no matter the OS and system and without dot net core dependencies. Sorry I had no time.


# Assumptions
* Based on the example given, and even though it is not explicit in the requirements, I assumed that whenever an event arrives with a previous timestamp than the last processed one, that event is ignored. In the example this happens in line *"type': 'ProductUpdated', 'id': 1, 'timestamp': 128, 'stock': 7}"*. The event is the only one which has timestamp newer than the last processed one and has no impact on the example output file.

# Code structure
For the implementation, I have chosen to go for .Net Core 1.1 (DNC).
There are 3 projects in the solution, one for the console application, another for the logic (I/O and domain) and a third one with unit and integration tests mixed altogether.

# Design choices
* Cheating - I used two external libraries:
  * **Newtonsoft.Json** to read the input file - Without it I would have had to write quite a lot of non-interesting code, so I cheated and added it (on the other hand I do not know which kind of facilities has Python Standard Library to read Json out of the box)
  * **Moq** to create Moqs for unit testing. This one is a must to write unit tests. Hope the goal of the exercise was not to see a bare implementation of a mocking framework ;).
* Traditional - My design is a plain old traditional console application. Something like the [sagas](https://msdn.microsoft.com/en-us/magazine/mt742866.aspx) or [Akka](http://getakka.net/) could be a good fit for this kind of application and worth exploring (or spiking).
* Integration tests in favour of unit tests. I decided to skip several unit tests in favour of integration tests. Since requirements were fixed and non arguable, I favoured the overall integration tests other than having unit tests for every single class. Also because of lack of time. In a multi-person, long living project I would not be comfortable doing this.
* Domain explicitness over reusability - As per my implementation, InputEvents, OutputEvents and SyncActions could have been forced to be almost the same (IStockEvent). I still have decided to have one class for each and deal with them in an explicit way in the engine so the domain is explicit and it adheres literally to your exercise statement and words. In situations where this can be reused and such reusability compensates the cognitive effort to deal with the abstraction then I go for it.
* InputLinesToIStockEventsFactory as a switch - This could have been implemented as a chain of responsibilities but, since the possible actions are fixed and don't seem to be changing in the short term, I opted by a plain old switch.


# Bonus task
##### How would you design architecture for system supporting synchronization of hundreds of millions of items in near real time? How would you deploy your solution, what components (like databases, or cache) would be required in your architecture?
I would shard the information.

I would expect that at some level we'd have the concept of seller (being a seller a unique entity which manages a set of product trees). To begin with, it would expect that sharding at a seller level would be a good starting point. Sharding could be achieved at several levels depending on the data distribution:
* Create one single datastore and add the concept of TenantId in it. In a single API, request all calls to provide the tenant Id parameter and forward this information to the datastore. This would require a sharded datastore and most probably several API instances to scale to millions of items in near real time. This option could be expensive in resources, as there would be a huge datastore to mantain.
* Create one datastore and one API per tenant all operating in the same dedicated private network (probably virtual). This option could be expensive in resources, specially if tenants have low volume. 
Despite the cost, one of the most interesting advantages of this approach is the data separation between customers. In case of security breach a customer would have no access to the other customers data.  
Last but not least, simplicity is always a nice to have from a maintenance point of view.
* Create several datastores, each one containing information from several tenants, and create one API instance (or multiple) for each datastore . One tenant would always go to the same datastore, something that would probably be managed via load balancer/gateway. In case one tenant had volume enough to consume all resources of the API and/or datastore or even require more, then you could create a dedicated cluster for them and even upscale their resources (probably at a higher rate for them).
 
> In reality every single *ProductTree* has zero dependency with all the other *ProductTrees*, so this would be the smallest level at which you can shard

All three approaches are combinable with:
* LoadBalancer - In case of more horse-power required at API processing level, then adding API instances and a load balancer in from of them would be my recommendation
* Gateway - In case we do have a throttling limit for our customers, or want to SSL offload our requests once inside our network.
* Firewall - To defend the software against malicious attacks (DDOS for example)
* Queue - In case peak periods stresses our system to its knees, it could be wise to introduce a queuing system. All our input actions and output actions are commands that can be serialized and that, if executed in a concrete sequence, always yield the same results. All this characteristics make them very queue friendly, and we could have a design where the API limits itself to store commands in a queue (or various, one per tenant, or one per priority (stock updates to amounts lower than 5 could have higher prio than the others), you could make it as fancy as you want) and afterwards a deamon, or AWS lambda pick it and execute it when possible.

> I firmly believe in keeping thins as simple as possible, so I speak about a lot of possibilities here that I would include in the final implementation only if they are necessary.


##### Describe how would you implement asynchronous processing in multiple threads/processes of input events (in contrast to processing one by one).
First of all I would make my implementation async (it is not) and this is a must considering we will have an external datastore (which will require at least one network hop) instead of an in-memory one.
Afterwards I would create a coordinator element which processes chunks of input actions and groups them by elements whose sequence matters. 
Let's give a couple of examples:
* If all requests are for the same product tree, then all the requests must be executed sequentially, so zero parallelization available.
* If all requests are for different product trees, then they all can be parallelized, as they are all independent of each other.

Most probably though, the most common scenario is that some requests are from the same product tree and some others not, so we could think of an algorithm that created independent chunks of dependant actions and then the system executes all those chunks in parallel (of course, respecting the sequence of the actions).
As part of this algorithm, and taking benefit that the result of the actions is deterministic, we could even decide to skip some of them collapsing the sequences. For example, if a set of ProductUpdated actions is identified for a single product, we could ignore all of them except for the last one (as the last one is setting the final value for the stock of the product, no matter the previous stock)

Example of algorithm:
1. Given N processors,
2. for each inputAction
2.1 Move it to chunk inputAction.TenantId module N
3. for each chunk 
3.1 Collapse redundant actions
3.2 Execute actions in corresponding processor

>  Considering implementation is async, this would probably perform even better with more chunks than processors, but this would require performance testing for fine-tuning. 

> The algorithm could split chunks by several criterias (tenantId, productTree, family of productTrees). Selecting one or the other would depend on data distribution. 

### Pending
In  a real world project, several other things would be missing in the code and repository as it is nowadays:
1. **Continuous Delivery/Continuous Integration** - The code to compile, build and run test and static code analysis (hopefully inside a Docker container) should be included in the repository, so every developer can checkout and start working anytime on any machine. Also, the CD pipeline could be included (or else stored in another repo).
2. **Logging** - Logging information is a must to find out problems in production. As of now there's no logging
3. **Versioning** - Version management is a must, specially once you go into CD world to track the multiple versions of your code. In general I follow [semver](http://semver.org/) which is has been a good fit for most of the projects I've worked on.
4. **Monitoring** - As of now there's no monitoring configured. In a real-world API having something like [NewRelic](https://newrelic.com/) or [app insights](https://azure.microsoft.com/en-us/services/application-insights/) is a must to keep track of your production state.
5. **Exception management** - Current exception management is poor. I tend to use DI interceptors to manage exceptions and logging. Had no time to do it and prioritized other stuff but anyway, without that part it is not possible to go live.
6. **Documentation** - If the application is to be consumed from the outside world or other colleagues from our company, then some kind of documentation is necessary, also input validation.
7. **Networking** - Current implementation has no network dependencies as everything is in memory. A real one will have it. To handle and minimize potential network errors (with retrials and circuit breakers for example), usage of [Polly](https://github.com/App-vNext/Polly) or similar solution is a must, especially in the cloud. 
8. **Security** - Binaries would have to be strong signed so we are sure that no one is poking around with the binaries that reach production.
9. **Scalability**
* Which kind of performance do we expect? 
* And workload? Will a single instance be enough? Do we expect to escalate horizontally at any moment in time? If so, Kubernetes or cloud machine templates should also be created from an infrastructure point of view. 
* Is there any seasonality in the usage? Is the same hardware enough always or should we plan for expansion on peak periods?
9. **Load Test** - Of course, once we know about the scalability we can have Load tests to validate the behavior of the app under expected workload. These tests should also be under source control, and can also be used to stress test the system looking for its weakest link. 
10. **Infrastructure as a code** - The infrastructure part (Azure ARM template to host and run the API/datastores, the network configuration, ...) should also be created and managed altogether with this repository if this is to become a micro-service.
11. **PACTs** - In case of a microservices architecture, I would expect all [PACT](https://github.com/SEEK-Jobs/pact-net) contracts to be included in the repository and executed as part of CI.
12. **Rebasing the pull-request** - I have left all my commits as is. In a normal PR I would rebase them to make my merge more readable and consistent.
