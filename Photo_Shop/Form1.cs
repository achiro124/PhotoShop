namespace Photo_Shop
{
    public partial class Form1 : Form
    {
        private Image pictureBoxImage;
        private Image copyPicrureBoxImage;
        private List<Image> listImages = new List<Image>();

        int wg = 0;
        private List<GroupBox> groupBoxes = new List<GroupBox>();
        private List<PictureBox> pictureBoxes = new List<PictureBox>();
        private List<ComboBox> comboBoxes = new List<ComboBox>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Button_AddImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog.Filter = "Картинки (png, jpg, bmp, gif) |*.png;*.jpg;*.bmp;*.gif|All files (*.*)|*.*";
            //saveFileDialog1.Filter = "Картинки (png, jpg, bmp, gif) |*.jpg;*.bmp;*.gif|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    try
                    {
                        listImages.Add(new Image(file));
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
                        Size = new Size(265, 196),
                        Location = new Point(5, n == 1 ? wg : wg + 210),
                        Text = "Изображение №" + (n).ToString()
                        
                    };

                    var pictureBox = new PictureBox
                    {
                        Name = "pictureBox" + (n).ToString(),
                        Size = new Size(180, 120),
                        BorderStyle = BorderStyle.FixedSingle,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Location = new Point(70, 26),
                        Image = (Bitmap)listImages[listImages.Count - 1].Img.Clone()
                    };
                    pictureBox.Click += new EventHandler(Button_Image_Click);
                    pictureBoxes.Add(pictureBox);
                    groupBox.Controls.Add(pictureBox);

                    var comboBox = new ComboBox
                    {
                        Name = "comboBox" + (n).ToString(),
                        Location = new Point(70, 150),
                        Size = new Size(180, 24),
                        Items = { "нет", "сумма", "разность", "среднее арифм.", "умножение", "минимум", "максимум" },
                        SelectedItem = "нет",
                    };
                    comboBox.SelectedValueChanged += new EventHandler(Change_ComboBox);
                    comboBoxes.Add(comboBox);
                    groupBox.Controls.Add(comboBox);

                    groupBoxes.Add(groupBox);
                    panel1.Controls.Add(groupBox);
                    wg = groupBox.Location.Y;
                }
                if (listImages.Count > 0)
                {
                    //pictureBox1.Image = (Bitmap)listImages[listImages.Count - 1].Img.Clone();
                    //pictureBoxImage = listImages[listImages.Count - 1];
                    //copyPicrureBoxImage = pictureBoxImage;
                }
                    
            }
        }

        private void Button_Image_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            for(int i = 0; i < pictureBoxes.Count; i++)
            {
                if (pictureBox != null && pictureBox?.Name == pictureBoxes[i].Name)
                {
                    pictureBox1.Image = (Bitmap)pictureBoxes[i].Image.Clone();
                    //pictureBoxImage = listImages[i];
                    //copyPicrureBoxImage = pictureBoxImage;
                    //comboBoxes[i].SelectedItem = "нет";
                    break;
                }
            }
        }
        private void Change_ComboBox(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.BlackQuad(new Bitmap(990, 900));
            pictureBoxImage = new Image((Bitmap)pictureBox1.Image.Clone());
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            for (int i = listImages.Count - 1; i >= 0; i--)
            {
                Image image;
                pictureBoxImage.ChangeImg((Bitmap)pictureBox1.Image.Clone());
                switch (comboBoxes[i].SelectedIndex)
                {
                    case 1:
                        {
                            image = pictureBoxImage + listImages[i];
                            pictureBox1.Image = (Bitmap)image.Img.Clone();
                            break;
                        };
                    case 2://разность
                        {
                            image = pictureBoxImage - listImages[i]; ;
                            pictureBox1.Image = (Bitmap)image.Img.Clone();
                            break;
                        };
                    case 3://среднее арифметическое
                        {
                
                            break;
                        };
                    case 4://умножение
                        {
                            image = pictureBoxImage * listImages[i]; ;
                            pictureBox1.Image = (Bitmap)image.Img.Clone();
                            break;
                        };
                    case 5://минимум
                        {
                            break;
                        };
                    case 6://максимум
                        {
                            break;
                        };
                    default:
                        {
                            //pictureBox1.Image = (Bitmap)pictureBoxImage.Img.Clone();
                            break;
                        }
                }
            }
        }
    }
}