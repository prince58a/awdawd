namespace BookLibrary.WinForms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.Button del;
        private System.Windows.Forms.Button edit;
        private System.Windows.Forms.Button author;
        private System.Windows.Forms.Button genre;
        private System.Windows.Forms.Button year;
        private System.Windows.Forms.ComboBox AuthorSearchComboBox;
        private System.Windows.Forms.ComboBox GenereSearchComboBox;
        private System.Windows.Forms.ComboBox YearSearchComboBox;
        private System.Windows.Forms.TextBox idSearchTextBox;
        private System.Windows.Forms.Button searchByIdButton;
        private System.Windows.Forms.Button resetSearchButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Label labelPageInfo;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Дефолт инициализация объектов винформы
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            add = new Button();
            del = new Button();
            edit = new Button();
            author = new Button();
            genre = new Button();
            year = new Button();
            AuthorSearchComboBox = new ComboBox();
            GenereSearchComboBox = new ComboBox();
            YearSearchComboBox = new ComboBox();
            idSearchTextBox = new TextBox();
            searchByIdButton = new Button();
            resetSearchButton = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            pictureBox1 = new PictureBox();
            btnPrevPage = new Button();
            btnNextPage = new Button();
            labelPageInfo = new Label();
            label4 = new Label();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(26, 25);
            dataGridView1.Margin = new Padding(5, 4, 5, 4);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(842, 371);
            dataGridView1.TabIndex = 0;
            dataGridView1.DataBindingComplete += DataGridView1_DataBindingComplete;
            // 
            // add
            // 
            add.BackgroundImageLayout = ImageLayout.Stretch;
            add.Location = new Point(386, 600);
            add.Margin = new Padding(5, 4, 5, 4);
            add.Name = "add";
            add.Size = new Size(154, 53);
            add.TabIndex = 8;
            add.Text = "Добавить";
            add.UseVisualStyleBackColor = true;
            // 
            // del
            // 
            del.BackgroundImageLayout = ImageLayout.Stretch;
            del.Location = new Point(694, 600);
            del.Margin = new Padding(5, 4, 5, 4);
            del.Name = "del";
            del.Size = new Size(154, 53);
            del.TabIndex = 10;
            del.Text = "Удалить";
            del.UseVisualStyleBackColor = true;
            // 
            // edit
            // 
            edit.BackgroundImageLayout = ImageLayout.Stretch;
            edit.Location = new Point(540, 600);
            edit.Margin = new Padding(5, 4, 5, 4);
            edit.Name = "edit";
            edit.Size = new Size(154, 53);
            edit.TabIndex = 9;
            edit.Text = "Редактировать";
            edit.UseVisualStyleBackColor = true;
            // 
            // author
            // 
            author.BackgroundImageLayout = ImageLayout.Stretch;
            author.Location = new Point(386, 467);
            author.Margin = new Padding(5, 4, 5, 4);
            author.Name = "author";
            author.Size = new Size(154, 47);
            author.TabIndex = 4;
            author.Text = "По автору";
            author.UseVisualStyleBackColor = true;
            // 
            // genre
            // 
            genre.BackgroundImageLayout = ImageLayout.Stretch;
            genre.Location = new Point(540, 467);
            genre.Margin = new Padding(5, 4, 5, 4);
            genre.Name = "genre";
            genre.Size = new Size(154, 47);
            genre.TabIndex = 5;
            genre.Text = "По жанру";
            genre.UseVisualStyleBackColor = true;
            // 
            // year
            // 
            year.BackgroundImageLayout = ImageLayout.Stretch;
            year.Location = new Point(694, 467);
            year.Margin = new Padding(5, 4, 5, 4);
            year.Name = "year";
            year.Size = new Size(154, 47);
            year.TabIndex = 6;
            year.Text = "С года";
            year.UseVisualStyleBackColor = true;
            // 
            // AuthorSearchComboBox
            // 
            AuthorSearchComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            AuthorSearchComboBox.FormattingEnabled = true;
            AuthorSearchComboBox.Location = new Point(386, 520);
            AuthorSearchComboBox.Margin = new Padding(5, 4, 5, 4);
            AuthorSearchComboBox.Name = "AuthorSearchComboBox";
            AuthorSearchComboBox.Size = new Size(153, 28);
            AuthorSearchComboBox.TabIndex = 11;
            // 
            // GenereSearchComboBox
            // 
            GenereSearchComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            GenereSearchComboBox.FormattingEnabled = true;
            GenereSearchComboBox.Location = new Point(540, 520);
            GenereSearchComboBox.Margin = new Padding(5, 4, 5, 4);
            GenereSearchComboBox.Name = "GenereSearchComboBox";
            GenereSearchComboBox.Size = new Size(153, 28);
            GenereSearchComboBox.TabIndex = 12;
            // 
            // YearSearchComboBox
            // 
            YearSearchComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            YearSearchComboBox.FormattingEnabled = true;
            YearSearchComboBox.Location = new Point(694, 520);
            YearSearchComboBox.Margin = new Padding(5, 4, 5, 4);
            YearSearchComboBox.Name = "YearSearchComboBox";
            YearSearchComboBox.Size = new Size(153, 28);
            YearSearchComboBox.TabIndex = 13;
            // 
            // idSearchTextBox
            // 
            idSearchTextBox.Location = new Point(26, 467);
            idSearchTextBox.Margin = new Padding(5, 4, 5, 4);
            idSearchTextBox.Name = "idSearchTextBox";
            idSearchTextBox.Size = new Size(192, 27);
            idSearchTextBox.TabIndex = 1;
            // 
            // searchByIdButton
            // 
            searchByIdButton.BackgroundImageLayout = ImageLayout.Stretch;
            searchByIdButton.Location = new Point(26, 507);
            searchByIdButton.Margin = new Padding(5, 4, 5, 4);
            searchByIdButton.Name = "searchByIdButton";
            searchByIdButton.Size = new Size(90, 33);
            searchByIdButton.TabIndex = 2;
            searchByIdButton.Text = "Поиск";
            searchByIdButton.UseVisualStyleBackColor = true;
            // 
            // resetSearchButton
            // 
            resetSearchButton.BackgroundImageLayout = ImageLayout.Stretch;
            resetSearchButton.Location = new Point(129, 507);
            resetSearchButton.Margin = new Padding(5, 4, 5, 4);
            resetSearchButton.Name = "resetSearchButton";
            resetSearchButton.Size = new Size(90, 33);
            resetSearchButton.TabIndex = 3;
            resetSearchButton.Text = "Сброс";
            resetSearchButton.UseVisualStyleBackColor = true;
            resetSearchButton.Click += resetSearchButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.Location = new Point(383, 440);
            label1.Margin = new Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new Size(110, 23);
            label1.TabIndex = 14;
            label1.Text = "Поиск книг:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label2.Location = new Point(26, 440);
            label2.Margin = new Padding(5, 0, 5, 0);
            label2.Name = "label2";
            label2.Size = new Size(115, 23);
            label2.TabIndex = 15;
            label2.Text = "Поиск по ID:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label3.Location = new Point(386, 573);
            label3.Margin = new Padding(5, 0, 5, 0);
            label3.Name = "label3";
            label3.Size = new Size(190, 23);
            label3.TabIndex = 16;
            label3.Text = "Управление книгами:";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.jhoe;
            pictureBox1.Location = new Point(432, 669);
            pictureBox1.Margin = new Padding(5, 4, 5, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(419, 200);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 17;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // btnPrevPage
            // 
            btnPrevPage.BackgroundImageLayout = ImageLayout.Stretch;
            btnPrevPage.Location = new Point(233, 399);
            btnPrevPage.Margin = new Padding(4, 3, 4, 3);
            btnPrevPage.Name = "btnPrevPage";
            btnPrevPage.Size = new Size(121, 29);
            btnPrevPage.TabIndex = 20;
            btnPrevPage.Text = "Предыдущая";
            btnPrevPage.UseVisualStyleBackColor = true;
            // 
            // btnNextPage
            // 
            btnNextPage.BackgroundImageLayout = ImageLayout.Stretch;
            btnNextPage.Location = new Point(361, 399);
            btnNextPage.Margin = new Padding(4, 3, 4, 3);
            btnNextPage.Name = "btnNextPage";
            btnNextPage.Size = new Size(132, 29);
            btnNextPage.TabIndex = 21;
            btnNextPage.Text = "Следующая";
            btnNextPage.UseVisualStyleBackColor = true;
            // 
            // labelPageInfo
            // 
            labelPageInfo.Location = new Point(26, 399);
            labelPageInfo.Margin = new Padding(4, 0, 4, 0);
            labelPageInfo.Name = "labelPageInfo";
            labelPageInfo.Size = new Size(201, 20);
            labelPageInfo.TabIndex = 22;
            labelPageInfo.Text = "Страница 1";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 16F);
            label4.Location = new Point(6, 733);
            label4.Margin = new Padding(5, 0, 5, 0);
            label4.Name = "label4";
            label4.Size = new Size(403, 37);
            label4.TabIndex = 18;
            label4.Text = "Какой же он крутой, я не могу";
            // 
            // button1
            // 
            button1.BackgroundImageLayout = ImageLayout.Stretch;
            button1.Font = new Font("Segoe UI", 14F);
            button1.ImageAlign = ContentAlignment.BottomCenter;
            button1.Location = new Point(26, 548);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Size = new Size(328, 105);
            button1.TabIndex = 23;
            button1.Text = "ЧЕРТ ВОЗЬМИ ДА 🐻🐻🐻🐻🐻";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(872, 661);
            Controls.Add(button1);
            Controls.Add(label4);
            Controls.Add(pictureBox1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(resetSearchButton);
            Controls.Add(searchByIdButton);
            Controls.Add(idSearchTextBox);
            Controls.Add(YearSearchComboBox);
            Controls.Add(GenereSearchComboBox);
            Controls.Add(AuthorSearchComboBox);
            Controls.Add(year);
            Controls.Add(genre);
            Controls.Add(author);
            Controls.Add(edit);
            Controls.Add(del);
            Controls.Add(add);
            Controls.Add(dataGridView1);
            Controls.Add(btnPrevPage);
            Controls.Add(btnNextPage);
            Controls.Add(labelPageInfo);
            Margin = new Padding(5, 4, 5, 4);
            MaximumSize = new Size(890, 918);
            MinimumSize = new Size(890, 698);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Библиотека книг";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void DataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.Columns.Contains("GenreId"))
            {
                dataGridView1.Columns["GenreId"].Visible = false;
            }
            if (dataGridView1.Columns.Contains("Id"))
            {
                dataGridView1.Columns["Id"].Width = 30;
                dataGridView1.Columns["Id"].HeaderText = "ID";
            }
            if (dataGridView1.Columns.Contains("Title"))
            {
                dataGridView1.Columns["Title"].Width = 280;
                dataGridView1.Columns["Title"].HeaderText = "Название";
            }
            if (dataGridView1.Columns.Contains("Author"))
            {
                dataGridView1.Columns["Author"].Width = 165;
                dataGridView1.Columns["Author"].HeaderText = "Автор";
            }
            if (dataGridView1.Columns.Contains("Year"))
            {
                dataGridView1.Columns["Year"].Width = 50;
                dataGridView1.Columns["Year"].HeaderText = "Год";
            }
            if (dataGridView1.Columns.Contains("Genre"))
            {
                dataGridView1.Columns["Genre"].Width = 125;
                dataGridView1.Columns["Genre"].HeaderText = "Жанр";
            }


        }

        private PictureBox pictureBox1;
        private Label label4;
        private Button button1;
    }

}