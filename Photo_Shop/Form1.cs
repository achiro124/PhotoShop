namespace Photo_Shop
{
    public partial class Form1 : Form
    {
        private Image picterBoxImage;
        private List<Image> listImages = new List<Image>();

        int wg = 0;
        private List<GroupBox> groupBoxes = new List<GroupBox>();
        private List<PictureBox> pictureBoxes = new List<PictureBox>();
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
                    //pictureBox.Click
                    pictureBoxes.Add(pictureBox);
                    
                    groupBox.Controls.Add(pictureBox);
                    groupBoxes.Add(groupBox);
                    panel1.Controls.Add(groupBox);
                    wg = groupBox.Location.Y;
                }
                if (listImages.Count > 0)
                {
                    pictureBox1.Image = (Bitmap)listImages[listImages.Count - 1].Img.Clone();
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
                    break;
                }
            }
        }
    }
}