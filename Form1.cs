using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Project_compiler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string inputstring = textBox1.Text;
            string Keywords = @"\b(int|float|string|read|write|repeat|until|if|elseif|else|then|return|endl|main)\b";
            string Identifiers = @"\b[A-Za-z][A-Za-z0-9]*\b";
            string numbers = @"\b [0-9]+(\.[0-9]+)?\b";
            string operators = @"(\+|\-|\*|\/|:=|<|>|=|<>|&&|\|\|)";
            string Symbol = @"[; {} () , ]";

            string masterPattern = $"{Keywords}|{Identifiers}|{numbers}|{operators}|{Symbol}";
            DataTable dt = new DataTable();
            dt.Columns.Add("Lexeme");
            dt.Columns.Add("Token Type");

            MatchCollection matches = Regex.Matches(inputstring, masterPattern);

            foreach (Match m in matches)
            {
                
                string lex = m.Value;
                if (string.IsNullOrWhiteSpace(lex))
                    continue;
                string type = "";
                if (Regex.IsMatch(lex, Keywords))
                    type = "Keywords";
                else if (Regex.IsMatch(lex, numbers))
                    type = "numbers";
                else if (Regex.IsMatch(lex, Identifiers))
                    type = "Identifiers";
                else if (lex == ":=") type = "Assignment_Op";

                else if (lex == "=") type = "Equal_Op";

                else if (lex == "<>") type = "Not_Equal_Op";

                else if (lex == "+") type = "Plus_Op";

                else if (lex == "-") type = "Minus_Op";

                else if (lex == "*") type = "Multiply_Op";

                else if (lex == "/") type = "Divide_Op";

                else if (lex == ">") type = "Greater_Than_Op";

                else if (lex == "<") type = "Less_Than_Op";

                else if (lex == "&&") type = "AND_Op";

                else if (lex == "||") type = "OR_Op";

                else if (lex == ";") type = "Semicolon";
                else if (lex == ",") type = "Comma";
                else if (lex == "(") type = "Left_Paren";
                else if (lex == ")") type = "Right_Paren";
                else if (lex == "{") type = "Left_Brace";
                else if (lex == "}") type = "Right_Brace";

                else type = "Unknown";

                dt.Rows.Add(lex, type);
            }
            dataGridView1.DataSource = dt;
        }
    }
}
