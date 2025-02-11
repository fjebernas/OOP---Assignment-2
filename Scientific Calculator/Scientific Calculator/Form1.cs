﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scientific_Calculator
{
    public partial class sciCal : Form
    {
        Image menuImage = Properties.Resources.face;
        Image cancelImage = Properties.Resources.cancel;
        

        Color normalButtonsColor = ColorTranslator.FromHtml("#1A1919");
        Color formBackColor = ColorTranslator.FromHtml("#2A262E");

        private bool isCollapsed;
        private bool isPanel1Showing = false;
        private bool isPanel2Showing = false;

        bool newEntry = false;
        bool flagFirstOperand = false;
        bool flagSecondOperand = false;
        bool isFinished = false;
        bool isDoingAnother = false;
        bool isDoingXraisedtoY = false;
        bool isDoingXrootofY = false;
        bool isDoingCustomlog = false;
        bool isDoingModulus = false;
        bool isFirstset = true;

        double firstOperand;
        double secondOperand;
        double result;
        double memoryNumber = 0;

        string theOperator;

        public sciCal()
        {
            InitializeComponent();
        }

        private void sciCal_Load(object sender, EventArgs e)
        {
            this.ActiveControl = btnEquals;
            this.BackColor = formBackColor;
            this.Opacity = 0.97;
            
            panelSlidingMenu.BackColor = formBackColor;
            //List<Button> MenuButtons = new List<Button> {
            //btnMenu1, btnMenu2, btnMenu3, btnMenu4, btnMenu5, btnMenu6, btnMenu7, btnMenu8, btnMenu9, btnMenu10, btnMenu11};

            //foreach (Button x in MenuButtons)
            //{
            //    x.BackColor = formBackColor;
            //}


            panelDropDown2.BackColor = normalButtonsColor;
            btnSin.BackColor = normalButtonsColor;
            btnCos.BackColor = normalButtonsColor;
            btnTan.BackColor = normalButtonsColor;
            btnSinh.BackColor = normalButtonsColor;
            btnCosh.BackColor = normalButtonsColor;
            btnTanh.BackColor = normalButtonsColor;

            foreach (Control x in Controls)
            {
                if (x.Tag == "ColorThisFormBackColor")
                {
                    x.BackColor = formBackColor;
                }
            }
            foreach (Control x in Controls)
            {
                if (x.Tag == "normalBtn")
                {
                    x.BackColor = normalButtonsColor;
                }
            }
        }
       
        private void btnCE_Click(object sender, EventArgs e)
        {
            displayTxtBox.Text = "0";

            if (isFinished)
            {
                labelEquation.Text = "";
            }
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            displayTxtBox.Text = "0";
            labelEquation.Text = "";
            firstOperand = 0;
            secondOperand = 0;

            flagFirstOperand = false;
            flagSecondOperand = false;
            newEntry = false;
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            if (displayTxtBox.Text.Length > 1)
            {
                displayTxtBox.Text = displayTxtBox.Text.Substring(0, displayTxtBox.Text.Length - 1);
            } else if (displayTxtBox.Text.Length == 1)
            {
                displayTxtBox.Text = "0";
            }
        }

        private void btnSquared_Click(object sender, EventArgs e)
        {
            double num = Convert.ToDouble(displayTxtBox.Text);

            if (isFirstset)
            {
                displayTxtBox.Text = Convert.ToString(Math.Pow(Convert.ToDouble(displayTxtBox.Text), 2));

                labelEquation.Text = "sqr(" + num.ToString() + ") =";
            } 
            else if (!isFirstset)
            {
                displayTxtBox.Text = Convert.ToString(Math.Pow(Convert.ToDouble(displayTxtBox.Text), 3));

                labelEquation.Text = "cube(" + num.ToString() + ") =";
            }
        }

        private void btnExpToY_Click(object sender, EventArgs e)
        {
            OperationSetter();

            if (isFirstset)
            {
                theOperator = "x raised to y";
                isDoingXraisedtoY = true;
            } 
            else if (!isFirstset)
            {
                theOperator = "x root y";
                isDoingXrootofY = true;
            }
        }

        private void btnCubed_Click(object sender, EventArgs e)
        {
            displayTxtBox.Text = Convert.ToString(Convert.ToDouble(displayTxtBox.Text) * Convert.ToDouble(displayTxtBox.Text) * Convert.ToDouble(displayTxtBox.Text));
        }

        private void btnMS_Click(object sender, EventArgs e)
        {
            memoryNumber = Convert.ToDouble(displayTxtBox.Text);

            labelMemoryIndicator.Visible = true;

            btnMC.Enabled = true;
            btnMR.Enabled = true;
            btnMadd.Enabled = true;
            btnMsub.Enabled = true;
        }

        private void btnMR_Click(object sender, EventArgs e)
        {
            displayTxtBox.Text = Convert.ToString(memoryNumber);
        }

        private void btnMC_Click(object sender, EventArgs e)
        {
            memoryNumber = 0;

            labelMemoryIndicator.Visible = false;

            btnMC.Enabled = false;
            btnMR.Enabled = false;
            btnMadd.Enabled = false;
            btnMsub.Enabled = false;
        }

        private void btnMadd_Click(object sender, EventArgs e)
        {
            memoryNumber += Convert.ToDouble(displayTxtBox.Text);
        }

        private void btnMsub_Click(object sender, EventArgs e)
        {
            memoryNumber -= Convert.ToDouble(displayTxtBox.Text);
        }

        private void btnPi_Click(object sender, EventArgs e)
        {
            displayTxtBox.Text = Convert.ToString(Math.PI);
        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            if (displayTxtBox.Text[0] != '-' && displayTxtBox.Text != "0")
            {
                displayTxtBox.Text = (displayTxtBox.Text).Insert(0, "-");
            }
            else if (displayTxtBox.Text[0] == '-' && displayTxtBox.Text != "0")
            {
                displayTxtBox.Text = displayTxtBox.Text.Substring(1);
            }
        }

        private void btnDecimalPoint_Click(object sender, EventArgs e)
        {
            if (!displayTxtBox.Text.Contains(".") || newEntry)
                if (newEntry)
                {
                    displayTxtBox.Text = "0.";
                    newEntry = false;
                }
                else
                {
                    displayTxtBox.Text = displayTxtBox.Text + ".";
                }
        }

        //Operators
        private void btnOperator_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (flagFirstOperand && flagSecondOperand)
            {
                btnEquals.PerformClick();
                OperationSetter();
                theOperator = button.Text;
                labelEquation.Text = displayTxtBox.Text + " " + theOperator + " ";
            } 
            else
            {
                OperationSetter();
                
                theOperator = button.Text;
                labelEquation.Text = displayTxtBox.Text + " " + theOperator + " ";
            }

            if (isFinished)
            {
                isDoingAnother = true;
            }
        }
        private void OperationSetter()
        {
                firstOperand = Convert.ToDouble(displayTxtBox.Text);

                flagFirstOperand = true;
                newEntry = true;
        }

        private void btnNum_Click(object sender, EventArgs e)
        {
            Button numBtn = (Button)sender;

            if (isFinished && !isDoingAnother)
            {
                displayTxtBox.Text = "0";
                labelEquation.Text = "";
                isFinished = false;
            }

            if (displayTxtBox.Text.Length < 14)
            {
                if (newEntry || displayTxtBox.Text == "0")
                {
                    displayTxtBox.Text = "";
                    newEntry = false;
                }

                if (flagFirstOperand)
                {
                    flagSecondOperand = true;
                }

                displayTxtBox.Text += numBtn.Text;
            }
        }


        private void btnEquals_Click(object sender, EventArgs e)
        {
            if (flagFirstOperand && flagSecondOperand)
            {
                secondOperand = Convert.ToDouble(displayTxtBox.Text);

                if (isDoingXraisedtoY)
                {
                    labelEquation.Text = Convert.ToString(firstOperand) + " raised to " + Convert.ToString(secondOperand) + " is";
                    isDoingXraisedtoY = false;
                } 
                else if (isDoingXrootofY)
                {
                    labelEquation.Text = Convert.ToString(secondOperand) + " root of " + Convert.ToString(firstOperand) + " is";
                    isDoingXrootofY = false;
                }
                else if (isDoingCustomlog)
                {
                    labelEquation.Text = Convert.ToString(firstOperand) + " log base " + Convert.ToString(secondOperand) + " =";
                    isDoingXrootofY = false;
                }
                else if (isDoingModulus)
                {
                    labelEquation.Text = Convert.ToString(firstOperand) + " modulus " + Convert.ToString(secondOperand) + " =";
                    isDoingModulus = false;
                }
                else
                {
                    labelEquation.Text = labelEquation.Text + Convert.ToString(secondOperand) + " =";
                }
                

                switch (theOperator)
                {
                    case "+":
                        result = firstOperand + secondOperand;
                        displayTxtBox.Text = Convert.ToString(result);
                        break;

                    case "-":
                        result = firstOperand - secondOperand;
                        displayTxtBox.Text = Convert.ToString(result);
                        break;

                    case "*":
                        result = firstOperand * secondOperand;
                        displayTxtBox.Text = Convert.ToString(result);
                        break;

                    case "÷":
                        result = firstOperand / secondOperand;
                        displayTxtBox.Text = Convert.ToString(result);
                        break;

                    case "x raised to y":
                        result = Math.Pow(firstOperand, secondOperand);
                        displayTxtBox.Text = Convert.ToString(result);
                        break;

                    case "x root y":
                        result = Math.Pow(firstOperand, 1/secondOperand);
                        displayTxtBox.Text = Convert.ToString(result);
                        break;

                    case "x log base y":
                        result = Math.Log(firstOperand, secondOperand);
                        displayTxtBox.Text = Convert.ToString(result);
                        break;

                    case "modulus":
                        result = firstOperand % secondOperand;
                        displayTxtBox.Text = Convert.ToString(result);
                        break;

                    default:
                        displayTxtBox.Text = displayTxtBox.Text;
                        break;
                }

                flagFirstOperand = false;
                flagSecondOperand = false;
                isFinished = true;
                isDoingAnother = false;
            }
        }


        // Dec 6 and onwards

        private void btnSin_Click(object sender, EventArgs e)
        {
            double num = Convert.ToDouble(displayTxtBox.Text);

            double degrees = (Convert.ToDouble(displayTxtBox.Text) * Math.PI) / 180;
            displayTxtBox.Text = Math.Sin(degrees).ToString("N5");

            labelEquation.Text = "sin(" + num.ToString() + "°) =";
        }

        private void btnCos_Click(object sender, EventArgs e)
        {
            double num = Convert.ToDouble(displayTxtBox.Text);

            double degrees = (Convert.ToDouble(displayTxtBox.Text) * Math.PI) / 180;
            displayTxtBox.Text = Math.Cos(degrees).ToString("N5");

            labelEquation.Text = "cos(" + num.ToString() + "°) =";
        }

        private void btnAbs_Click(object sender, EventArgs e)
        {
            double num = Convert.ToDouble(displayTxtBox.Text);

            double absValue = Convert.ToDouble(displayTxtBox.Text);
            absValue = Math.Abs(absValue);
            displayTxtBox.Text = Convert.ToString(absValue);

            labelEquation.Text = "abs(" + num.ToString() + ") =";
        }

        private void btnTan_Click(object sender, EventArgs e)
        {
            double num = Convert.ToDouble(displayTxtBox.Text);

            double degrees = (Convert.ToDouble(displayTxtBox.Text) * Math.PI) / 180;
            displayTxtBox.Text = Math.Tan(degrees).ToString("N5");

            labelEquation.Text = "tan(" + num.ToString() + "°) =";
        }

        //drop down panel functions
        private void btnTrigoDropdown_Click(object sender, EventArgs e)
        {
            if (!isPanel1Showing)
            {
                panelDropDown1.Visible = true;
                isPanel1Showing = true;
            } 
            else if (isPanel1Showing)
            {
                panelDropDown1.Visible = false;
                isPanel1Showing = false;
            }
        }

        private void btnModeDropdown_Click(object sender, EventArgs e)
        {
            if (!isPanel2Showing)
            {
                panelDropDown2.Visible = true;
                isPanel2Showing = true;
            }
            else if (isPanel2Showing)
            {
                panelDropDown2.Visible = false;
                isPanel2Showing = false;
            }
        }

        private void sciCal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0)
            {
                btnNum_Click(btn0, e);
            }
            else if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
            {
                btnNum_Click(btn1, e);
            }
            else if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
            {
                btnNum_Click(btn2, e);
            }
            else if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
            {
                btnNum_Click(btn3, e);
            }
            else if (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4)
            {
                btnNum_Click(btn4, e);
            }
            else if (e.KeyCode == Keys.D5 || e.KeyCode == Keys.NumPad5)
            {
                btnNum_Click(btn5, e);
            }
            else if (e.KeyCode == Keys.D6 || e.KeyCode == Keys.NumPad6)
            {
                btnNum_Click(btn6, e);
            }
            else if (e.KeyCode == Keys.D7 || e.KeyCode == Keys.NumPad7)
            {
                btnNum_Click(btn7, e);
            }
            else if (e.KeyCode == Keys.D8 || e.KeyCode == Keys.NumPad8)
            {
                btnNum_Click(btn8, e);
            }
            else if (e.KeyCode == Keys.D9 || e.KeyCode == Keys.NumPad9)
            {
                btnNum_Click(btn9, e);
            }
            else if (e.KeyCode == Keys.Enter)
            {
                btnEquals.PerformClick();
            }
            else if (e.KeyCode == Keys.Add)
            {
                btnPlus.PerformClick();
            }
            else if (e.KeyCode == Keys.Subtract)
            {
                btnMinus.PerformClick();
            }
            else if (e.KeyCode == Keys.Multiply)
            {
                btnTimes.PerformClick();
            }
            else if (e.KeyCode == Keys.Divide)
            {
                btnDivide.PerformClick();
            }
            else if (e.KeyCode == Keys.Decimal)
            {
                btnDecimalPoint.PerformClick();
            }
            else if (e.KeyCode == Keys.Back)
            {
                btnBackspace.PerformClick();
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            timerSlidingMenu.Start();

            if (isCollapsed)
            {
                btnMenu.Image = cancelImage;
                labelCalcuMode.Visible = false;
            } 
            else
            {
                btnMenu.Image = menuImage;
                labelCalcuMode.Visible = true;
            }
        }

        private void timerSlidingMenu_Tick(object sender, EventArgs e)
        {
            

            if (isCollapsed)
            {
                panelSlidingMenu.Width += 40;
                if(panelSlidingMenu.Size == panelSlidingMenu.MaximumSize)
                {
                    timerSlidingMenu.Stop();
                    isCollapsed = false;
                }
            }
            else
            {
                panelSlidingMenu.Width -= 40;
                if (panelSlidingMenu.Size == panelSlidingMenu.MinimumSize)
                {
                    timerSlidingMenu.Stop();
                    isCollapsed = true;
                }
            }
        }

        private void btnOneOverX_Click(object sender, EventArgs e)
        {
            double num = Convert.ToDouble(displayTxtBox.Text);

            displayTxtBox.Text = Convert.ToString(1 / Convert.ToDouble(displayTxtBox.Text));

            labelEquation.Text = "1 / (" + num.ToString() + ") =";
        }

        private void btnRadical_Click(object sender, EventArgs e)
        {
            double num = Convert.ToDouble(displayTxtBox.Text);

            if (isFirstset)
            {
                displayTxtBox.Text = Convert.ToString(Math.Sqrt(Convert.ToDouble(displayTxtBox.Text)));

                labelEquation.Text = "sqrt(" + num.ToString() + ") =";
            } 
            else if (!isFirstset)
            {
                displayTxtBox.Text = Convert.ToString(Math.Pow(Convert.ToDouble(displayTxtBox.Text), 0.3333333333333333));

                labelEquation.Text = "cuberoot(" + num.ToString() + ") =";
            }
            
        }

        private void btnFactorial_Click(object sender, EventArgs e)
        {
            if (!displayTxtBox.Text.Contains(".") )
            {
                labelEquation.Text = "fact(" + displayTxtBox.Text + ") =";

                double result = 1;

                for (double i = Convert.ToDouble(displayTxtBox.Text); i != 1; i--)
                {
                    result = result * i;
                }

                displayTxtBox.Text = Convert.ToString(result);
            }
            else if (displayTxtBox.Text == "0")
            {
                displayTxtBox.Text = "1";
            }
            else
            {
                //displayTxtBox.Font = new Font(displayTxtBox.Font.FontFamily, 20);
                //displayTxtBox.Text = "Must be a whole number";
                //displayTxtBox.Font = new Font(displayTxtBox.Font.FontFamily, 32);

                MessageBox.Show("Must be a whole number");
                displayTxtBox.Text = "0";
            }
        }

        private void btn_MouseEnter(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (button.Tag == "numpad")
            {
                button.BackColor = ColorTranslator.FromHtml("#476384");
            } 
            else if (button.Tag == "normalBtn")
            {
                button.BackColor = ColorTranslator.FromHtml("#312C48");
            } 
            else if (button.Tag == "equals")
            {
                button.BackColor = ColorTranslator.FromHtml("#1D2046");
                button.Font = new Font(button.Font.FontFamily, 20);
            }
        }

        private void btn_MouseLeave(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (button.Tag == "numpad")
            {
                button.BackColor = ColorTranslator.FromHtml("#000000");
            } 
            else if (button.Tag == "normalBtn")
            {
                button.BackColor = normalButtonsColor;
            }
            else if (button.Tag == "equals")
            {
                button.BackColor = Color.FromArgb(19, 21, 45);
                button.Font = new Font(button.Font.FontFamily, 15);
            }
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            double num = Convert.ToDouble(displayTxtBox.Text);

            if (isFirstset)
            {
                displayTxtBox.Text = Convert.ToString(Math.Log10(Convert.ToDouble(displayTxtBox.Text)));

                labelEquation.Text = "log(" + num.ToString() + ") =";
            } 
            else if (!isFirstset)
            {
                OperationSetter();
                theOperator = "x log base y";
                isDoingCustomlog = true;
            }
            
        }

        private void btnTenRaisedToX_Click(object sender, EventArgs e)
        {
            double num = Convert.ToDouble(displayTxtBox.Text);

            if (isFirstset)
            {
                displayTxtBox.Text = Convert.ToString(Math.Pow(10, Convert.ToDouble(displayTxtBox.Text)));

                labelEquation.Text = "10 ^ (" + num.ToString() + ") =";
            } 
            else if (!isFirstset)
            {
                displayTxtBox.Text = Convert.ToString(Math.Pow(2, Convert.ToDouble(displayTxtBox.Text)));

                labelEquation.Text = "2 ^ (" + num.ToString() + ") =";
            }
            
        }

        private void btnExp_Click(object sender, EventArgs e)
        {
            labelEquation.Text = "e ^ (" + displayTxtBox.Text + ") =";

            displayTxtBox.Text = Convert.ToString(Math.Exp(Convert.ToDouble(displayTxtBox.Text)));
        }

        private void btnFE_Click(object sender, EventArgs e)
        {
            double num = Convert.ToDouble(displayTxtBox.Text);
            displayTxtBox.Text = num.ToString("G2", CultureInfo.InvariantCulture);
        }

        private void btnShift_Click(object sender, EventArgs e)
        {
            if (isFirstset)
            {
                btnSquared.Text = "𝑥³";
                btnRadical.Text = "∛𝑥";
                btnExpToY.Text = "ʸ√𝑥";
                btnTenRaisedToX.Text = "2ˣ";
                btnLog.Text = "logᵧ𝑥";
                btnLn.Text = "eˣ";

                isFirstset = false;
            } 
            else if (!isFirstset)
            {
                btnSquared.Text = "𝑥²";
                btnRadical.Text = "√𝑥";
                btnExpToY.Text = "𝑥ʸ";
                btnTenRaisedToX.Text = "10ˣ";
                btnLog.Text = "log";
                btnLn.Text = "ln";

                isFirstset = true;
            }
           
        }

        private void btnLn_Click(object sender, EventArgs e)
        {
            double num = Convert.ToDouble(displayTxtBox.Text);

            if (isFirstset)
            {
                displayTxtBox.Text = Convert.ToString(Math.Log(Convert.ToDouble(displayTxtBox.Text), Math.E));

                labelEquation.Text = "ln(" + num.ToString() + ") =";
            } 
            else if (!isFirstset)
            {
                displayTxtBox.Text = Convert.ToString(Math.Pow(Math.E, Convert.ToDouble(displayTxtBox.Text)));

                labelEquation.Text = "e ^ (" + num.ToString() + ") =";
            }
            
        }

        private void btnMod_Click(object sender, EventArgs e)
        {
            OperationSetter();
            theOperator = "modulus";
            isDoingModulus = true;
        }

        private void btnSinh_Click(object sender, EventArgs e)
        {
            labelEquation.Text = "sinh(" + displayTxtBox.Text + "°) =";
            displayTxtBox.Text = Math.Sinh(Convert.ToDouble(displayTxtBox.Text)).ToString();
        }

        private void btnCosh_Click(object sender, EventArgs e)
        {
            labelEquation.Text = "cosh(" + displayTxtBox.Text + "°) =";
            displayTxtBox.Text = Math.Cosh(Convert.ToDouble(displayTxtBox.Text)).ToString();
        }

        private void btnTanh_Click(object sender, EventArgs e)
        {
            labelEquation.Text = "tanh(" + displayTxtBox.Text + "°) =";
            displayTxtBox.Text = Math.Tanh(Convert.ToDouble(displayTxtBox.Text)).ToString();
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            displayTxtBox.Text = rnd.Next(1, 101).ToString();
        }

        private void btnE_Click(object sender, EventArgs e)
        {
            displayTxtBox.Text = Math.E.ToString();
        }
    }
}
