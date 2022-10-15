# MemCal
#### Video Demo: https://youtube.com
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

My main criticism of my own project and process was in retrospect I felt like I spent
more time learning the GUI markup language and the Avalonia framework, rather than the
"programming" side of things. It would have also been interesting to incorporate a
persistence layer using EntityFramework and SQLite or similar, but I felt if I didn't
finally just down tools and submit something I'd keep tinkering forever.
