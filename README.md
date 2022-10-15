# MemCal
#### Video Demo: https://www.youtube.com/watch?v=h786mmUOwtY
#### GitHub Repo: https://github.com/benrnorman/MemCal
#### Description:
A simple cross-platform calculator.

This project was chosen to give me a chance to learn .NET, the MVVM model, and develop
a progrma with a GUI. I decided to learn C# for the language. For the GUI, I experimented
with MAUI and Uno Platform, before finally settling Avalonia as it generally had better
documentation and a broader community to bounce off of. MemCal can theoretically run on
Windows, MacOS, Linux, iOS, Android, and WASM, although it has only been tested on the
three desktop environments.

The C# language felt fairly intuitive and compared well with the languages learnt during
the course. Coming from a more HTML/CSS world, the XAML markup language felt somewhat less
intuitive, though fairly straight-forward in the end. The biggest painpoint I encountered
was the data-binding and the Model-View-ViewModel structure, which took a while before it
"clicked".

The specific domain-related challenge I set myself of this project was to add some small
feature compared to the default simple calculators offered by Windows and Mac. Comparing
them, their either avoided the order of operations issue by resolving each simple expression
on operator input, or they did not keep a history of performed calculations. Even when
they did track previous calculations, it was read-only. So, in this program I aimed to:

* Allow more complex expressions that respected the order of operations.
* Kept a history of calculations performed.
* Allowed inserting historical calculations into the working expression.

## Project structure
### Assets
Contains the only custom-made icon used in the project. All other icons are FontAwesome
SVGs pulled in using a NuGet package.

### DataTypes
The DataTypes directory contains custom datatypes defined for this project.
#### Enums
Enumerated types that define a set of named values.
* Operation.cs - the enum created for this project is the Operation enum, defining the
specific operations allowed by this program and the character used to denote them. This was
used for error-checking and to prevent anything malicious potentially being submitted.

### Models
The Models directory contains the definitions for the classes that make up the domain model.
* Calculation.cs - the Calculation class is defined here. It represents a mathemtical
expression and the result. The ToString method is overridden to print out a nicer representation
of the full calculation, for display in the history log.

### ViewModels
The ViewModels used throughout the project represent and abstraction of the view that
exposes certain properties and methods without having to worry about the specific implementation
in the view.
* ViewModelBase.cs - the ViewModelBase is view model that all other view models inherit. It
sets up the reactivity for the models for the data-binding between the view and viewmodel.
* MainWindowViewModel.cs - the MainWindowViewModel is the specific implementation of the view
model for the main window. It intialises several commands to be sent to the view so that inputs
can be bound and sent back to the view model. It tracks the working expression, the historical
calculations, and various flags for determining the correct input.

### Views
The Views are the specific implementation of the GUI that a user can interact with. They
consist of a XAML final definining the view, and a code-behind file that implements some
view-specific operations.
* MainWindow.axaml - the MainWindow XAML file that defines the layout of the GUI and binds
certain UI elements to interactions the user can perform, or displaying output for the user.
* MainWindow.axaml.cs - the MainWindow code-behind file, that defines some view-specific logic
such as the history view toggle and handling the key inputs.

## Reflections
My main criticism of my own project and process was in retrospect I felt like I spent
more time learning the MVVM model, GUI markup language, and the Avalonia framework, rather
than the "programming" side of things. It would have also been interesting to incorporate a
persistence layer using EntityFramework and SQLite or similar, but I felt if I didn't
finally just down tools and submit something I'd keep tinkering forever.
