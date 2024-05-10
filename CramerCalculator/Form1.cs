namespace CramerCalculator
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-') ||
                (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-') ||
                (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0))
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-') ||
                (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0))
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-') ||
                (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0))
            {
                e.Handled = true;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-') ||
                (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0))
            {
                e.Handled = true;
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-') ||
                (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0))
            {
                e.Handled = true;
            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-') ||
                (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0))
            {
                e.Handled = true;
            }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-') ||
                (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0))
            {
                e.Handled = true;
            }
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-') ||
                 (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0))
            {
                e.Handled = true;
            }
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-') ||
                (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0))
            {
                e.Handled = true;
            }
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-') ||
                 (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0))
            {
                e.Handled = true;
            }
        }

        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-') ||
                (e.KeyChar == '-' && (sender as TextBox).Text.Length > 0))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
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
    }
}
