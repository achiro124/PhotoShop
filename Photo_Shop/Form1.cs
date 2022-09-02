using System.Windows.Forms;

namespace Photo_Shop
{
    public partial class Form1 : Form
    {
        private Image pictureBoxImage;
        private Image copyPicrureBoxImage;
        private List<Image> listImages = new();
        private List<Image> copyListImages = new();

        int wg = 0;
        int index;
        private List<GroupBox> groupBoxes = new();
        private List<PictureBox> pictureBoxes = new();
        private List<ListBox> listBoxes = new();
        private List<Button> buttonBoxes = new();
        private List<CheckBox> checkBoxes = new();
        private List<NumericUpDown> numericUpDownBoxes = new();

        private MaskParametrs MaskParametrs;
        public Form1()
        {
            InitializeComponent();
        }
        private void Button_Add_Image_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog(); 
            openFileDialog.Multiselect = true;
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog.Filter = "Картинки (png, jpg, bmp, gif) |*.png;*.jpg;*.bmp;*.gif|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    try
                    {
                        listImages.Add(new Image(file));
                        copyListImages.Add(new Image(file));
                    }
                    catch
                    {
                        MessageBox.Show("Не удалось открыть картинку", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    int n = listImages.Count();
                    var groupBox = new GroupBox
                    {
                        Name = "groupBox" + (n).ToString(),
                        BackColor = Color.Gray,
                        Size = new Size(265, 226),
                        Location = new Point(5, n == 1 ? wg : wg + 240),
                        Text = "Изображение №" + (n).ToString()
                        
                    };

                    var pictureBox = new PictureBox
                    {
                        Name = "pictureBox" + (n).ToString(),
                        Size = new Size(195, 120),
                        BorderStyle = BorderStyle.FixedSingle,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Location = new Point(55, 26),
                        Image = (Bitmap)listImages[listImages.Count - 1].Img.Clone()
                    };
                    pictureBox.Click += new EventHandler(Button_Image_Click);
                    pictureBoxes.Add(pictureBox);
                    groupBox.Controls.Add(pictureBox);

                    var listBox = new ListBox
                    {
                        Name = "listBox" + (n).ToString(),
                        Location = new Point(65, 150),
                        Size = new Size(180, 24),
                        Items = { "нет", "сумма", "разность", "среднее арифм.", "умножение", "минимум", "максимум" },
                        SelectedItem = "нет",
                    };
                    listBox.SelectedValueChanged += new EventHandler(Change_ComboBox);
                    listBoxes.Add(listBox);
                    groupBox.Controls.Add(listBox);

                    var buttonBox = new Button
                    {
                        Name = "buttonBox" + (n).ToString(),
                        Location = new Point(238, 0),
                        Size = new Size(28, 28),
                        Text = "X"
                    };
                    buttonBox.Click += new EventHandler(Button_Delete_Img_Click);
                    buttonBoxes.Add(buttonBox);
                    groupBox.Controls.Add(buttonBox);

                    var checkbox1 = new CheckBox
                    {
                        Name = "checkBox1-" + (n + 1).ToString(),
                        Location = new Point(9, 40),
                        Size = new Size(40, 23),
                        Font = new Font("Times New Roman", 9),
                        Text = "R",
                        Checked = true,
                    };
                    checkbox1.Click += new EventHandler(Change_CheckBox_Click);
                    var checkbox2 = new CheckBox
                    {
                        Name = "checkBox2-" + (n + 1).ToString(),
                        Location = new Point(9, 80),
                        Size = new Size(40, 23),
                        Font = new Font("Times New Roman", 9),
                        Text = "G",
                        Checked = true
                    };
                    checkbox2.Click += new EventHandler(Change_CheckBox_Click);
                    var checkbox3 = new CheckBox
                    {
                        Name = "checkBox3-" + (n + 1).ToString(),
                        Location = new Point(9, 120),
                        Size = new Size(40, 23),
                        Font = new Font("Times New Roman", 9),
                        Text = "B",
                        Checked = true
                    };


                    checkbox3.Click += new EventHandler(Change_CheckBox_Click);
                    checkBoxes.Add(checkbox1);
                    checkBoxes.Add(checkbox2);
                    checkBoxes.Add(checkbox3);
                    groupBox.Controls.Add(checkbox1);
                    groupBox.Controls.Add(checkbox2);
                    groupBox.Controls.Add(checkbox3);


                    var label1 = new Label
                    {
                        Name = "labell" + (n + 1).ToString(),
                        Location = new Point(50, 188),
                        Size = new Size(115, 18),
                        Font = new Font("Times New Roman", 10),
                        Text = "Прозрачность:",
                    };
                    var numUpDown = new NumericUpDown
                    {
                        Name = "numUpDown" + (n + 1).ToString(),
                        Location = new Point(165, 185),
                        Size = new Size(50, 22),
                        Value = 100,
                        Maximum = 100
                    };
                    var label2 = new Label
                    {
                        Name = "labelll" + (n + 1).ToString(),
                        Location = new Point(220, 188),
                        Size = new Size(18, 18),
                        Font = new Font("Times New Roman", 10),
                        Text = "%"
                    };
                    numUpDown.ValueChanged += new EventHandler(Change_NumericUpDown);
                    numericUpDownBoxes.Add(numUpDown);
                    groupBox.Controls.Add(label1);
                    groupBox.Controls.Add(label2);
                    groupBox.Controls.Add(numUpDown);

                    groupBoxes.Add(groupBox);
                    panel1.Controls.Add(groupBox);
                    wg = groupBox.Location.Y;
                }
                if (listImages.Count > 0)
                {
                    pictureBox1.Image = (Bitmap)listImages[listImages.Count - 1].Img.Clone();
                    pictureBoxImage = copyListImages[copyListImages.Count - 1];
                    index = copyListImages.Count - 1;
                    //copyPicrureBoxImage = pictureBoxImage;

                }

                сохранитьКакToolStripMenuItem.Enabled = true;
            }
        }
        private void Button_Image_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            for(int i = 0; i < pictureBoxes.Count; i++)
            {
                if (pictureBox != null && pictureBox?.Name == pictureBoxes[i].Name)
                {
                    pictureBox1.Image = (Bitmap)copyListImages[i].Img.Clone();
                    pictureBoxImage = copyListImages[i];
                    index = i;
                    //copyPicrureBoxImage = pictureBoxImage;
                    listBoxes[i].SelectedItem = "нет";
                    break;
                }
            }
        }
        private void Button_Delete_Img_Click(object sender, EventArgs e)
        {
            int loc = 0;
            Button temp = sender as Button;
            var name = temp?.Name;
            int index = 0;
            for (int i = 0; i < listImages.Count(); i++)
            {
                if (buttonBoxes[i].Name == name)
                {
                    index = i;
                    groupBoxes[i].Dispose();
                    groupBoxes.RemoveAt(i);
                    listImages.RemoveAt(i);
                    copyListImages.RemoveAt(i);
                    listBoxes.RemoveAt(i);
                    numericUpDownBoxes.RemoveAt(i);
                    pictureBoxes.RemoveAt(i);
                    buttonBoxes.RemoveAt(i);
                    for(int j = 0; j < 3; j ++)
                    {
                    
                        checkBoxes.RemoveAt(i * 3);
                    }
                   break;
                }
            }
            if (index != 0)
            {
                loc = groupBoxes[index - 1].Location.Y;
            }
            for (int i = index; i < listImages.Count(); i++)
            {
                groupBoxes[i].Location = new Point(5, i == 0 ? loc : loc + 240);
                groupBoxes[i].Name = "groupBox" + (i + 1).ToString();
                groupBoxes[i].Text = "Изображение №" + (i + 1).ToString();
                pictureBoxes[i].Name = "pictureBox" + (i + 1).ToString();
                loc = groupBoxes[i].Location.Y;
                
            }
            if(groupBoxes.Count() > 0)
                wg = groupBoxes.Last().Location.Y;
        }
        private void Change_ComboBox(object sender, EventArgs e)
        {
            AllOperation();
        }
        private void Change_NumericUpDown(object sender, EventArgs e)
        {
            NumericUpDown numericUpDown = sender as NumericUpDown;
            for (int i = 0; i < listImages.Count(); i++)
            {
                if (numericUpDown.Name == numericUpDownBoxes[i].Name)
                {
                    int clarity = (int)numericUpDownBoxes[i].Value;
                    copyListImages[i] = copyListImages[i].ChangeClarity(clarity);
                    if (i == index)
                        pictureBoxImage = copyListImages[i];
                    AllOperation();
                    break;
                }
            }
        }
        private void Button_Mask_Click(object sender, EventArgs e)
        {
            MaskParametrs = new(pictureBox1, pictureBoxImage);
            MaskParametrs.Show();
        }
        private void Change_CheckBox_Click(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            bool R = true, G = true, B = true;
            int k = 0;
            for (int i = 0; i < checkBoxes.Count(); i++)
            {
                if (checkBox.Name == checkBoxes[i].Name)
                {
                    k = i / 3;
                    break;
                }
            }
            if (!checkBoxes[3 * k].Checked)
            {
                R = false;
            }
            if (!checkBoxes[3 * k + 1].Checked)
            {
                G = false;
            }
            if (!checkBoxes[3 * k + 2].Checked)
            {
                B = false;
            }
            copyListImages[k] = listImages[k].ChangeColorChanel(R,G,B);
            if ((int)numericUpDownBoxes[k].Value != 100) //Проверка, изменяется ли прозрачность у фотографии или нет
            {
                int clarity = (int)numericUpDownBoxes[k].Value;
                copyListImages[k] = copyListImages[k].ChangeClarity(clarity);
            }

            if (k == index)
                pictureBoxImage = copyListImages[k];
            AllOperation();
        }
        private void AllOperation()
        {
            pictureBox1.Image = (Bitmap)pictureBoxImage.Img.Clone();
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            for (int i = listImages.Count - 1; i >= 0; i--)
            {
                Image image;
                copyPicrureBoxImage = new((Bitmap)pictureBox1.Image.Clone());
                switch (listBoxes[i].SelectedIndex)
                {
                    case 1: //Сложение
                        {
                            image = Image.OperationSum(copyPicrureBoxImage, copyListImages[i]);
                            pictureBox1.Image = (Bitmap)image.Img?.Clone();
                            break;
                        };
                    case 2://разность
                        {
                            image = Image.OperationDiff(copyPicrureBoxImage, copyListImages[i]);
                            pictureBox1.Image = (Bitmap)image.Img?.Clone();
                            break;
                        };
                    case 3://среднее арифметическое
                        {
                            image = Image.OperationAvg(copyPicrureBoxImage, copyListImages[i]);
                            pictureBox1.Image = (Bitmap)image.Img?.Clone();
                            break;
                        };
                    case 4://умножение
                        {
                            image = Image.OperationMult(copyPicrureBoxImage, copyListImages[i]);
                            pictureBox1.Image = (Bitmap)image.Img?.Clone();
                            break;
                        };
                    case 5://минимум
                        {
                            image = Image.OperationMin(copyPicrureBoxImage, copyListImages[i]);
                            pictureBox1.Image = (Bitmap)image.Img?.Clone();
                            break;
                        };
                    case 6://максимум
                        {
                            image = Image.OperationMax(copyPicrureBoxImage, copyListImages[i]);
                            pictureBox1.Image = (Bitmap)image.Img?.Clone();
                            break;
                        };
                }
            }
        }
        private void Save_Img_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(pictureBox1.Image is not null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Картинки (png, jpg, bmp, gif) |*.jpg;*.bmp;*.gif|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                    return;
                string filename = saveFileDialog.FileName;
                pictureBox1.Image.Save(filename);
                MessageBox.Show("Файл сохранен");
            }
            else
            {
                MessageBox.Show("Не удалось сохранить картинку", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}