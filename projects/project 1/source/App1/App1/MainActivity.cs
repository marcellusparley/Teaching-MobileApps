using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Collections;

namespace App1
{
    [Activity(Label = "App1", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);
            Button buttonCalc = FindViewById<Button>(Resource.Id.buttonCalculate);

            buttonCalc.Click += CalcTextField;
            button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
        }

        private void CalcTextField(object sender, EventArgs e)
        {
            EditText textField = FindViewById<EditText>(Resource.Id.calcTextField);
            Stack calcStack = new Stack();
            char opSymbol;
            double temp, result = 0.0;
            string[] items = textField.Text.Split(' ');

            foreach (string item in items)
            {
                if (double.TryParse(item, out temp))
                {
                    calcStack.Push(temp);
                }
                else if (calcStack.Count >= 2)
                {
                    char.TryParse(item, out opSymbol);
                    switch (item)
                    {
                        case "+":
                            temp = (double)calcStack.Pop();
                            result = temp + (double)calcStack.Pop();
                            calcStack.Push(result);
                            break;
                        case "-":
                            temp = (double)calcStack.Pop();
                            result = (double)calcStack.Pop() - temp;
                            calcStack.Push(result);
                            break;
                        case "*":
                            temp = (double)calcStack.Pop();
                            result = (double)calcStack.Pop() * temp;
                            calcStack.Push(result);
                            break;
                        case "/":
                            temp = (double)calcStack.Pop();
                            result = (double)calcStack.Pop() / temp;
                            calcStack.Push(result);
                            break;
                        default:
                            break;
                    }
                    
                }
            }

            if (calcStack.Count == 1)
            {
                result = (double)calcStack.Pop();
                textField.Text = result.ToString();
            }
            else
            {
                textField.Text = "Something went wrong!";
            }

            //throw new NotImplementedException();
        }
    }
}

