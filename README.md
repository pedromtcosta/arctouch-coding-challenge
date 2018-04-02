# Build instructions
- Simply set the MovieApp.Android as the starting project and run the solution
- The MovieApp.iOS should also be working, but I currently don`t have a Mac OS to compile and test it

# Third-party libraries used:
- CSharpFunctionalExtensions (https://github.com/vkhorikov/CSharpFunctionalExtensions): provides helpful extensions for dealing with code in a functional way. Used basically so I can easily go for a success or error scenario (like populating data if success, and sending a error message if error)
- FluentAssertions (https://github.com/fluentassertions/fluentassertions): Used so I can have a nice and fluid way of doing Asserts in unit tests
- moq (https://github.com/moq/moq): For mocking in Unit Tests
- Prism (https://github.com/PrismLibrary/Prism): Choice of MVVM Framework for the project. Have nice abstractions to handle things like Navigation and Displaying Alerts