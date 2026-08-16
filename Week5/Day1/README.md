# Week 5 — Day 1: Unit Testing with xUnit

## Overview

Day 1 focused on the fundamentals of unit testing using xUnit in .NET.

The main goal was to create a test project, write unit tests using `[Fact]` and `[Theory]`, and practice the Arrange-Act-Assert (AAA) pattern.

## Topics Covered

- xUnit fundamentals
- `[Fact]` attribute
- `[Theory]` attribute
- `[InlineData]`
- Arrange-Act-Assert pattern
- Testing pure methods
- Running and verifying unit tests

## Test Project Setup

An xUnit test project was created and connected to the existing API project from Weeks 1–4 using a project reference.

This allows the test project to access and test classes and methods from the API project.

## Calculator Example

A simple `Calculator` class was used to practice unit testing.

```csharp
public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }
}

The Add method is a pure method because it does not depend on a database, API, or other external dependencies.

## Fact Tests :

Three [Fact] tests were created to test different scenarios:

Adding two positive numbers
Adding a positive and a negative number
Adding two negative numbers

Example:

[Fact]
public void Add_TwoPositiveNumbers_ReturnsCorrectSum()
{
    // Arrange
    var calculator = new Calculator();


    // Act
    var result = calculator.Add(5, 3);


    // Assert
    Assert.Equal(8, result);
}

## Theory Test

A [Theory] test was created to test the same method with multiple input cases.

[Theory]
[InlineData(5, 3, 8)]
[InlineData(5, -3, 2)]
[InlineData(-5, -3, -8)]
public void Add_ReturnsCorrectSum(int a, int b, int expected)
{
    // Arrange
    var calculator = new Calculator();


    // Act
    var result = calculator.Add(a, b);


    // Assert
    Assert.Equal(expected, result);
}

[InlineData] provides different sets of input values to the same test method.

Arrange-Act-Assert (AAA)

Each test follows the AAA pattern:

-Arrange

Prepare the objects and input data required for the test.

-Act

Execute the method being tested.

-Assert

Verify that the actual result matches the expected result.

Result

All written unit tests were successfully executed and passed.

The Day 1 hands-on lab was completed using xUnit, [Fact], [Theory], [InlineData], and the Arrange-Act-Assert pattern.


