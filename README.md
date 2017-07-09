# iasset.com coding test #

This is a technical test for iasset.com

It demonstrates the use of Angular 2 for the front-end and connects to a Web API service for the back end.
As a result, the front-end scripting is written in TypeScript. 
This solution doesn't bring any warnings from the compiler or Resharper.

The solution has been deployed to http://iasset.azurewebsites.net


Requirements:
1. Visual Studio 2015
2. TypeScript addon 2.4
3. SpecFlow Addon


It demonstrates the use of the following C# 6 features:
- Null-conditional operators
- String interpolation
- Index Initializers
- Auto-Property Initializer
- Expression-bodied function members


It demonstrates the use of other C# features:
- Async modifier and Await operator
- Generics
- Lambda and LINQ expressions
- Anonymous types


Design Patterns used:
- Adapter
- Dependency injection of Generics
- Decorators (implicit with Angular 2)
- Arrange Act Assert


For the testing:
- Moq for the mocking framework
- Business Driven Development (BDD)
- Gherkin syntax with SpecFlow


The following SOLID principles were used:
- [S]ingle Responsibility principle
- [I]nterface Segregation principle
- [D]ependency Inversion principle 


I chose Simple Injector for the DI as it does well in the IoC container benchmarks
http://www.palmmedia.de/blog/2011/8/30/ioc-container-benchmark-performance-comparison

 