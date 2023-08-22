# Implementation of Unit Test using Xunit and Moq in .NET Core 6 Web API


![img](https://miro.medium.com/v2/resize:fit:630/0*17z4hbMSKL4cxVEb.jpg)

We are going to discuss unit tests using xUnit and Moq here step-by-step  in detail, I suggest you read my following blog for a basic  understanding of unit test case

[Unit Test using xUnit in .NET Core 6 with the help of VS Code](https://medium.com/p/a4ccc3409610)

**Introduction**

• Unit Testing is a software design pattern that is used to test the smallest components in the software development phase.

• Unit Testing is used to validate the functionality which is to create  expected output before going to the production environment and QA Team.

• It helps to detect issues at the early phase of the software development cycle

• There are many unit test tools that are already present while using .NET Framework like xUnit, NUnit, and many more.

**xUnit**

• xUnit is a free and open-source Unit testing framework for .NET development

• xUnit has many features which provide for writing a clean and good unit test case.

• It has many attributes like Fact, Theory, and many more to write test  cases effectively and cleanly and also provides a mechanism to create  our own attribute

**Attributes of xUnit**

[Fact] attribute is used by xUnit in .NET which identifies the method for unit test

```
[Fact]
public void EvenNumberTest() {
    //Arrange
    var num = 6;
    //Act
    bool result = Mathematics.IsEvenNumber(num);
    //Assert
    Assert.True(result);
}
```

[Theory] attribute is used to supply parameters to the test method

```
[Theory]
[InlineData(5)]
public void OddNumberTest(int num) {
    //Act
    bool result = Mathematics.IsOddNumber(num);
    //Assert
    Assert.True(result);
}
```

**Test Pattern**

Arrange-Act-Assert is a great way to write clean and more readable unit test cases

Arrange

In the arrange section we setup and declare some inputs and configuration variable

Act

In the Act section, we put main things and functionality like method calls, API calls, and something like that

Assert

Assert checks expected outputs and check whether they will match our functional requirement or not

**Moq**

- Basically, Moq is the library that is used for mocking purposes.
- Suppose our application is dependent on one or more services at that time we  don’t need to initialize all the things related to that we just use the  Moq library to mock some classes and functionality with dummy data.

**Step 1)**

Create a new .NET Core API Project

![img](https://miro.medium.com/v2/resize:fit:700/0*XqXG18A2IWuAG7dI.png)

**Step 2)**

Configure your project

![img](https://miro.medium.com/v2/resize:fit:700/0*ml-phq5XXx5cRXN_.png)

**Step 3)**

Provide additional information about your project

![img](https://miro.medium.com/v2/resize:fit:700/0*M3Mr0MiNWHvnkiuk.png)

**Step 4)**

Project Structure

![img](https://miro.medium.com/v2/resize:fit:577/0*v7YMXMt6EHeWB_Mn.png)

**Step 5)**

Install Following NuGet Packages

![img](https://miro.medium.com/v2/resize:fit:700/0*aVYxZ4ReE5mJzUV_.png)

**Step 6)**

Create the Models folder and create a new class Product

<iframe src="https://medium.com/media/710384ed23ef401b5720224863ecb500" allowfullscreen="" frameborder="0" height="281" width="680" title="Product.cs" class="ek n fc dx bg" scrolling="no"></iframe>

**Step 7)**

Next, Create DbContextClass inside the Data folder for data manipulation

<iframe src="https://medium.com/media/98ae7949976dcf213f5b9c9e8f2fac17" allowfullscreen="" frameborder="0" height="435" width="680" title="DbContextClass.cs" class="ek n fc dx bg" scrolling="no"></iframe>

**Step 8)**

Later on, Create IProductService and ProductService class for abstraction and dependency injection inside the Services folder.

<iframe src="https://medium.com/media/604835345cdff35c306c695ef3da0515" allowfullscreen="" frameborder="0" height="303" width="680" title="IProductService.cs" class="ek n fc dx bg" scrolling="no"></iframe>

Create ProductService class

<iframe src="https://medium.com/media/248059881066b9f6c8ef3a3d07b1ff00" allowfullscreen="" frameborder="0" height="919" width="680" title="ProductService.cs" class="ek n fc dx bg" scrolling="no"></iframe>

**Step 9)**

After that, Create a new ProductController

<iframe src="https://medium.com/media/0d061ebe2dfa483afcd91116ad3fb6f0" allowfullscreen="" frameborder="0" height="963" width="680" title="ProductController.cs" class="ek n fc dx bg" scrolling="no"></iframe>

**Step 10)**

Add connection string inside app setting file

<iframe src="https://medium.com/media/91735ce36b6eac6a70af48e61adebbf4" allowfullscreen="" frameborder="0" height="303" width="680" title="appsettings.json" class="ek n fc dx bg" scrolling="no"></iframe>

**Step 11)**

Next, register a few services inside Program Class

<iframe src="https://medium.com/media/b6c6c0ae18be15f8faf595c54df8908c" allowfullscreen="" frameborder="0" height="501" width="680" title="Program.cs" class="ek n fc dx bg" scrolling="no"></iframe>

**Step 12)**

Add migrations and update the database using the following entity framework command after executing that into the package manager console under the main project

> *add-migration “First”*
>
> *update-database*

**Step 13)**

Finally, run your application and you will see swagger UI and API endpoints

![img](https://miro.medium.com/v2/resize:fit:700/0*ZvokJDj6ONf66FyG.png)

This is all about the .NET Core Web API, Let’s create a new Xunit project inside which we use Moq to write the test cases.

**Step 1)**

Add a new Xunit project inside the existing solution

![img](https://miro.medium.com/v2/resize:fit:700/0*aSCWezPyvWoSVzeT.png)

**Step 2)**

Configure your new project

![img](https://miro.medium.com/v2/resize:fit:700/0*N80XeztRONayJGIx.png)

**Step 3)**

Provide some additional information

![img](https://miro.medium.com/v2/resize:fit:700/0*4Vyz6odDWJzu6c6Q.png)

**Step 4)**

Install Moq NuGet Package for mocking purpose

![img](https://miro.medium.com/v2/resize:fit:700/0*ms3rpdLztvvcSdkz.png)

**Step 5)**

Create UnitTestController Class

<iframe src="https://medium.com/media/ef681802d87e107d5a1924ff717cfab6" allowfullscreen="" frameborder="0" height="2437" width="680" title="UnitTestController.cs" class="ek n fc dx bg" scrolling="no"></iframe>

- Here, you can see first we add reference of our main project into the current unit test project
- We mock IProductService and create an instance inside the constructor
- Next, we write one test case which takes a list of a product
- Later on, we take a list of products from our custom method which is present in the same class at the bottom
- Next, set up the list of products for the product service with some mock data
- Also, our product controller is dependent on product service and because of  that, we pass the product service object inside the product controller  constructor to resolve some dependencies.
- In the act section, we call the ProductList method of the controller.
- Finally, in the assert section, we check actual and expected results using a few conditions

Similarly, all the test cases are worked step by step

**Step 6)**

Next, go to the Test Section at the top of Visual Studio and open the Test  Explorer inside that you can see all the test cases which we write  inside the UnitTestControllerClass

**Step 7)**

Final Project Structure

![img](https://miro.medium.com/v2/resize:fit:365/0*fLhbO6OLQVSJrtUA.png)

**Step 8)**

Finally, run your test cases and check if they will be worked properly or not,  also if you want to debug a test case simply right-click on the test  case and click on debug after attaching the debugger point inside the  test case

![img](https://miro.medium.com/v2/resize:fit:700/0*jfBIvpPyUDIypQwR.png)

**Conclusion**

We see the introduction of unit tests and some attributes with patterns.  After that, discussed Moq and its usage. Also, step-by-step  implementation of using .NET Core 6.