namespace CramerCalculator
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            BindKeyPressEventToTextBoxes();
            BindKeyDownEventToTextBoxes();
        }
        private void BindKeyPressEventToTextBoxes()
        {
            textBox1.KeyPress += TextBox_KeyPress;
            textBox2.KeyPress += TextBox_KeyPress;
            textBox3.KeyPress += TextBox_KeyPress;
            textBox4.KeyPress += TextBox_KeyPress;
            textBox5.KeyPress += TextBox_KeyPress;
            textBox6.KeyPress += TextBox_KeyPress;
            textBox7.KeyPress += TextBox_KeyPress;
            textBox8.KeyPress += TextBox_KeyPress;
            textBox9.KeyPress += TextBox_KeyPress;
            textBox10.KeyPress += TextBox_KeyPress;
            textBox11.KeyPress += TextBox_KeyPress;
            textBox12.KeyPress += TextBox_KeyPress;
        }

        private void BindKeyDownEventToTextBoxes()
        {
            textBox1.KeyDown += textBox_KeyDown;
            textBox2.KeyDown += textBox_KeyDown;
            textBox3.KeyDown += textBox_KeyDown;
            textBox4.KeyDown += textBox_KeyDown;
            textBox5.KeyDown += textBox_KeyDown;
            textBox6.KeyDown += textBox_KeyDown;
            textBox7.KeyDown += textBox_KeyDown;
            textBox8.KeyDown += textBox_KeyDown;
            textBox9.KeyDown += textBox_KeyDown;
            textBox10.KeyDown += textBox_KeyDown;
            textBox11.KeyDown += textBox_KeyDown;
            textBox12.KeyDown += textBox_KeyDown;
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox currentTextBox = sender as TextBox;

            if (currentTextBox != null)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    this.ActiveControl = null;
                }
                if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                {
                    int currentIndex = this.Controls.IndexOf(currentTextBox);
                    int currentColumn = currentIndex % 4;
                    
                    if (e.KeyCode == Keys.Left && currentColumn > -1)
                    {
                        for (int i = currentIndex + 1; i >= 0; i--)
                        {
                            if (this.Controls[i] is TextBox)
                            {
                                this.Controls[i].Focus();
                                break;
                            }
                        }
                    }
                    else if (e.KeyCode == Keys.Right && currentColumn < 4)
                    {
                        for (int i = currentIndex - 1; i < this.Controls.Count; i++)
                        {
                            if (this.Controls[i] is TextBox)
                            {
                                this.Controls[i].Focus();
                                break;
                            }
                        }
                    }
                    else if (e.KeyCode == Keys.Up)
                    {
                        int newIndex = currentIndex + 4;
                        if (newIndex >= 0 && newIndex < this.Controls.Count && this.Controls[newIndex] is TextBox)
                        {
                            this.Controls[newIndex].Focus();
                        }
                    }
                    else if (e.KeyCode == Keys.Down)
                    {
                        int newIndex = currentIndex - 4;
                        if (newIndex >= 0 && newIndex < this.Controls.Count && this.Controls[newIndex] is TextBox)
                        {
                            this.Controls[newIndex].Focus();
                        }
                    }

                    e.Handled = true;
                }
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-') ||
                (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SolveEquations();
        }

        private void SolveEquations()
        {
            double[,] coefficients = new double[3, 3];
            double[] constants = new double[3];

            double.TryParse(textBox1.Text, out coefficients[0, 0]);
            double.TryParse(textBox2.Text, out coefficients[0, 1]);
            double.TryParse(textBox3.Text, out coefficients[0, 2]);
            double.TryParse(textBox5.Text, out coefficients[1, 0]);
            double.TryParse(textBox6.Text, out coefficients[1, 1]);
            double.TryParse(textBox7.Text, out coefficients[1, 2]);
            double.TryParse(textBox9.Text, out coefficients[2, 0]);
            double.TryParse(textBox10.Text, out coefficients[2, 1]);
            double.TryParse(textBox11.Text, out coefficients[2, 2]);
            double.TryParse(textBox4.Text, out constants[0]);
            double.TryParse(textBox8.Text, out constants[1]);
            double.TryParse(textBox12.Text, out constants[2]);

            try
            {
                double[] solution = SolveCramer(coefficients, constants);
                if (solution != null)
                {
                    label15.Text = $"x1 = {solution[0]:F2}";
                    label16.Text = $"x2 = {solution[1]:F2}";
                    label17.Text = $"x3 = {solution[2]:F2}";
                    if (checkBox1.Checked) 
                    {
                        WriteResultToFile(solution, coefficients, constants);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private double[] SolveCramer(double[,] coefficients, double[] constants)
        {
            double determinant = CalculateDeterminant(coefficients);
            if (determinant == 0)
            {
                MessageBox.Show("Система уравнений вырожденная. Решение невозможно.");
                return null;
            }

            int n = constants.Length;
            double[] solution = new double[n];
            for (int i = 0; i < n; i++)
            {
                double[,] modifiedMatrix = ModifyMatrix(coefficients, constants, i);
                solution[i] = CalculateDeterminant(modifiedMatrix) / determinant;
            }
            return solution;
        }

        private double CalculateDeterminant(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            if (n != matrix.GetLength(1))
            {
                throw new ArgumentException("Матрица должна быть квадратной");
            }

            if (n == 1)
            {
                return matrix[0, 0];
            }
            else if (n == 2)
            {
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
            }
            else
            {
                double determinant = 0;
                for (int i = 0; i < n; i++)
                {
                    double[,] submatrix = new double[n - 1, n - 1];
                    for (int j = 1; j < n; j++)
                    {
                        for (int k = 0; k < n; k++)
                        {
                            if (k < i)
                            {
                                submatrix[j - 1, k] = matrix[j, k];
                            }
                            else if (k > i)
                            {
                                submatrix[j - 1, k - 1] = matrix[j, k];
                            }
                        }
                    }
                    determinant += (i % 2 == 0 ? 1 : -1) * matrix[0, i] * CalculateDeterminant(submatrix);
                }
                return determinant;
            }
        }
        private double[,] ModifyMatrix(double[,] coefficients, double[] constants, int index)
        {
            int n = coefficients.GetLength(0);
            double[,] modifiedMatrix = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    modifiedMatrix[i, j] = coefficients[i, j];
                }
            }
            for (int i = 0; i < n; i++)
            {
                modifiedMatrix[i, index] = constants[i];
            }
            return modifiedMatrix;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
        }

        private void ClearTextBoxes()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
            label15.Text = "";
            label16.Text = "";
            label17.Text = "";
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadTestExample();
        }
        private void LoadTestExample()
        {
            textBox1.Text = "3";    
            textBox2.Text = "2";    
            textBox3.Text = "3";    
            textBox4.Text = "14";   
            textBox5.Text = "2";    
            textBox6.Text = "-1";  
            textBox7.Text = "1";    
            textBox8.Text = "5"; 
            textBox9.Text = "3";   
            textBox10.Text = "4";  
            textBox11.Text = "-1"; 
            textBox12.Text = "5";
        }
        private void WriteResultToFile(double[] solution, double[,] coefficients, double[] constants)
        {
            try
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "result.txt");

                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.WriteLine("Изначальная система уравнений:");
                    WriteEquationsToFile(writer, coefficients, constants);
                    writer.WriteLine();

                    writer.WriteLine("Решение системы:");
                    writer.WriteLine($"x1 = {solution[0]:F2}");
                    writer.WriteLine($"x2 = {solution[1]:F2}");
                    writer.WriteLine($"x3 = {solution[2]:F2}");
                }

                MessageBox.Show("Результат записан в файл " + path);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при записи в файл: {ex.Message}");
            }
        }

        private void WriteEquationsToFile(StreamWriter writer, double[,] coefficients, double[] constants)
        {
            for (int i = 0; i < constants.Length; i++)
            {
                for (int j = 0; j < coefficients.GetLength(1); j++)
                {
                    writer.Write($"{coefficients[i, j]}x{j + 1} ");
                    if (j < coefficients.GetLength(1) - 1)
                    {
                        writer.Write("+ ");
                    }
                }
                writer.WriteLine($"= {constants[i]}");
            }
        }
    }
}
