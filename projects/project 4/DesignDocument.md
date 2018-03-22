# PA4: REVISITING PA3-VISION "BEAT GOOGLE VISION"
This application is a game for testing out the Google Vision Api. It is very simple. Basically it will let you capture an image with your device's camera or draw and image on a canvas and the app sends that image to google's cloud testing for labels. It then tries to guess the name of an item/thing featured in that image. The application is formated into a sort of guessing game with branching screens. The purpose, aside from meeting the homework requirement, is to test the abilities of the api quickly.

The new additions to this app are the Multi-touch drawing canvas as well as the ability to test your drawing with the api and see if it could guess what you were trying to draw.

## System Design 
This app requires use of the device's camera, internet and both reading and writing to external storage. It was tested on an Android 7.0 tablet and on Android 7.1 emulation. It's built to have a minimum requirement of Android 5.0 but this is untested. After taking the image the app might hang a bit as it loads, resizes and sends the image to the cloud and waits for results.

## Usage
Usage of this app is simple. Upon opening, you are greeted to the initial screen with a button to open your camera and a button to open the newly added drawing canvas which I will cover later(as seen below-left).

![Image of initial screen](first.png) ![Image of the guess screen](guess.png)

After taking and selecting the image from the device's camera app you are taken to the second screen where the app will try to guess the content of the image (above-right). Selecting 'yes' here will send you to the success screen where the app will gloat about winning (as seen below-left). The "success" screen has a button that will return to the initial screen. Selecting 'no' at the guess will send you to the input screen (pictured below-right) In which the app asks what the content is. Here you must input text to continue.

![Image of "success" screen](success.png) ![Image of input screen](no.png)

Hitting submit after entering something has two possible outcomes. Either the thing you entered was in the label detection results and the last screen will say you lost, or it was not in the results and you will have won. Both will again feature a button to return to the initial page.

![Image of loss outcome](inresult.png) ![Image of win outcome](win.png)

The drawing canvas button takes the users to a [spoiler]drawing canvas[/spoiler] where they can doodle about. The top of the drawing screen holds the control panel which features RGB slider-controlled color picker on the left and a number entry field to control the stroke width on the right. Each have buttons below to set the changes to the "paintbrush". Below that is a clear button which clears the canvas and a identify button which will send the image to the vision api and take the user to the outcome screen. The rest of the screen is dedicated to the canvas itself which can handle drawing with multiple fingers at once.

![Image of happy tree](happytree.png)

The newly added draw outcome screen asks the user what he/she intended to draw and then remarks on the drawing's accuracy. If the object is in the api results the app will show the percentage, if not it will just insult the user. In either case, pressing the submit button will also make the return button visible which, like it's counterpart in the camera section, will take the user back to the beginning.

![Image of good drawing outcome](drawright.png) ![Image of poor drawing outcome](drawwrong.png)

That's basically all there is to it. Also to note you should be able to use the navigate back button to safely go back a screen if you need to. -should- . Lastly try to get your camera target in the center of the frame because for whatever reason the app seems to rotate the image and it -may- be chopping off the top and bottom of the image.
