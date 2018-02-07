# Project 1 - A Simple Postfix Calculator
This application is just a simple postfix calculator I wrote to play around with Xamarin android development and to meet a homework requirement. It allows the user to calculate addition, subtraction, multiplication, and division between numbers including decimals. I'm not sure I'd expect it to be used or anything because at least for me, postfix calculation can be needlessly hard to read at times especially when more fully featured, infix calculators exist out there. Regardless, it was a fun little project to introduce myself with Android development and I think I picked up quite a bit about Xamarin and C# in a very short time span.

## System Design 
This app was designed mostly with vertical view in mind so that would be the best orientation to use it with. That is probably the only reccomended requirement, it doesn't use any of your device's hardware features and it doesn't need a substantial amount of resources or anything.

The design itself is fairly simple, the two main elements are the calculation field (TextView widget) above, which contains and displays the equation, and the number input field (EditText widget) below, which handles the entry of numbers and is where the result is sent after calculation. The rest of the UI are the buttons which handle inserting operations to the calculation field, clearing the calculation field, and the single button dedicated to sending numbers to the calculation field. Number entry is handled by the device's native numpad and allows for decimals.

## Usage
Usage is fairly simple, though admittedly had I gone and added the 0-9 and decimal buttons to the UI myself, it would be even simpler. 

To enter a number you just touch the text field that helpfully displays the 'Input numbers here...' hint text and your device's numpad should pop up. Use the numpad to type out your number and when satisfied hit enter to send it to the calculation field.

To enter an operation just hit one of the four operation buttons: +, -, *, /.

The clear button will reset the calculation field to a blank state.

Also note that since this is a postfix calculator you want to have at least two numbers preceding your operation or else it will not calculate anything. In this situation the calculation field will display 'Something went wrong!' which you can clear away and try again.
