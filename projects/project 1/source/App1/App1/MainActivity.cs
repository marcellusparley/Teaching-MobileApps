using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Collections.Generic;

namespace App1
{
    [Activity(Label = "Postfix Calculator", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button buttonCalc = FindViewById<Button>(Resource.Id.buttonCalculate);
            Button buttonEnter = FindViewById<Button>(Resource.Id.buttonNumEnter);
            Button buttonPlus = FindViewById<Button>(Resource.Id.buttonPlus);
            Button buttonMinus = FindViewById<Button>(Resource.Id.buttonMinus);
            Button buttonTimes = FindViewById<Button>(Resource.Id.buttonTimes);
            Button buttonDiv = FindViewById<Button>(Resource.Id.buttonDivide);
            Button buttonClear = FindViewById<Button>(Resource.Id.buttonClear);

            buttonCalc.Click += CalcTextField;
            buttonEnter.Click += EnterInputNumber;
            buttonPlus.Click += AddSymbol;
            buttonMinus.Click += AddSymbol;
            buttonTimes.Click += AddSymbol;
            buttonDiv.Click += AddSymbol;
            buttonClear.Click += ClearTextField;
        }

        private void ClearTextField(object sender, EventArgs e)
        {
            TextView t = FindViewById<TextView>(Resource.Id.calcTextField);
            t.Text = "";
        }

        private void AddSymbol(object sender, EventArgs e)
        {
            Button b = sender as Button;
            TextView t = FindViewById<TextView>(Resource.Id.calcTextField);
            t.Text += b.Text + " ";

        }

        private void EnterInputNumber(object sender, EventArgs e)
        {
            TextView textField = FindViewById<TextView>(Resource.Id.calcTextField);
            EditText numberField = FindViewById<EditText>(Resource.Id.numberInputField);
            textField.Text += numberField.Text + " ";
            numberField.Text = "";
        }

        private void CalcTextField(object sender, EventArgs e)
        {
            TextView textField = FindViewById<TextView>(Resource.Id.calcTextField);
            EditText numberField = FindViewById<EditText>(Resource.Id.numberInputField);
            Stack<double> calcStack = new Stack<double>();
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
                            temp = calcStack.Pop();
                            result = temp + calcStack.Pop();
                            calcStack.Push(result);
                            break;
                        case "-":
                            temp = calcStack.Pop();
                            result = calcStack.Pop() - temp;
                            calcStack.Push(result);
                            break;
                        case "*":
                            temp = calcStack.Pop();
                            result = calcStack.Pop() * temp;
                            calcStack.Push(result);
                            break;
                        case "/":
                            temp = calcStack.Pop();
                            result = calcStack.Pop() / temp;
                            calcStack.Push(result);
                            break;
                        default:
                            break;
                    }
                    
                }
            }

            if (calcStack.Count == 1)
            {
                result = calcStack.Pop();
                numberField.Text = result.ToString();
                textField.Text = "";
            }
            else
            {
                textField.Text = "Something went wrong!";
            }

            //throw new NotImplementedException();
        }
    }
}

