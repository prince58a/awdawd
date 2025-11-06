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
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(23, 25);
            dataGridView1.Margin = new Padding(4);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(749, 370);
            dataGridView1.TabIndex = 0;
            // 
            // add
            // 
            add.Location = new Point(343, 600);
            add.Margin = new Padding(4);
            add.Name = "add";
            add.Size = new Size(137, 53);
            add.TabIndex = 8;
            add.Text = "Добавить";
            add.UseVisualStyleBackColor = true;
            add.Click += BtnADD_Click;
            // 
            // del
            // 
            del.Location = new Point(617, 600);
            del.Margin = new Padding(4);
            del.Name = "del";
            del.Size = new Size(137, 53);
            del.TabIndex = 10;
            del.Text = "Удалить";
            del.UseVisualStyleBackColor = true;
            del.Click += BtnDEL_Click;
            // 
            // edit
            // 
            edit.Location = new Point(480, 600);
            edit.Margin = new Padding(4);
            edit.Name = "edit";
            edit.Size = new Size(137, 53);
            edit.TabIndex = 9;
            edit.Text = "Редактировать";
            edit.UseVisualStyleBackColor = true;
            edit.Click += BtnEDIT_Click;
            // 
            // author
            // 
            author.Location = new Point(343, 467);
            author.Margin = new Padding(4);
            author.Name = "author";
            author.Size = new Size(137, 47);
            author.TabIndex = 4;
            author.Text = "По автору";
            author.UseVisualStyleBackColor = true;
            author.Click += BtnSORTAuthor_Click;
            // 
            // genre
            // 
            genre.Location = new Point(480, 467);
            genre.Margin = new Padding(4);
            genre.Name = "genre";
            genre.Size = new Size(137, 47);
            genre.TabIndex = 5;
            genre.Text = "По жанру";
            genre.UseVisualStyleBackColor = true;
            genre.Click += BtnSORTGenre_Click;
            // 
            // year
            // 
            year.Location = new Point(617, 467);
            year.Margin = new Padding(4);
            year.Name = "year";
            year.Size = new Size(137, 47);
            year.TabIndex = 6;
            year.Text = "С года";
            year.UseVisualStyleBackColor = true;
            year.Click += BtnSORTYear_Click;
            // 
            // AuthorSearchComboBox
            // 
            AuthorSearchComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            AuthorSearchComboBox.FormattingEnabled = true;
            AuthorSearchComboBox.Location = new Point(343, 520);
            AuthorSearchComboBox.Margin = new Padding(4);
            AuthorSearchComboBox.Name = "AuthorSearchComboBox";
            AuthorSearchComboBox.Size = new Size(136, 28);
            AuthorSearchComboBox.TabIndex = 11;
            // 
            // GenereSearchComboBox
            // 
            GenereSearchComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            GenereSearchComboBox.FormattingEnabled = true;
            GenereSearchComboBox.Location = new Point(480, 520);
            GenereSearchComboBox.Margin = new Padding(4);
            GenereSearchComboBox.Name = "GenereSearchComboBox";
            GenereSearchComboBox.Size = new Size(136, 28);
            GenereSearchComboBox.TabIndex = 12;
            // 
            // YearSearchComboBox
            // 
            YearSearchComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            YearSearchComboBox.FormattingEnabled = true;
            YearSearchComboBox.Location = new Point(617, 520);
            YearSearchComboBox.Margin = new Padding(4);
            YearSearchComboBox.Name = "YearSearchComboBox";
            YearSearchComboBox.Size = new Size(136, 28);
            YearSearchComboBox.TabIndex = 13;
            // 
            // idSearchTextBox
            // 
            idSearchTextBox.Location = new Point(23, 467);
            idSearchTextBox.Margin = new Padding(4);
            idSearchTextBox.Name = "idSearchTextBox";
            idSearchTextBox.Size = new Size(171, 27);
            idSearchTextBox.TabIndex = 1;
            // 
            // searchByIdButton
            // 
            searchByIdButton.Location = new Point(23, 507);
            searchByIdButton.Margin = new Padding(4);
            searchByIdButton.Name = "searchByIdButton";
            searchByIdButton.Size = new Size(80, 33);
            searchByIdButton.TabIndex = 2;
            searchByIdButton.Text = "Поиск";
            searchByIdButton.UseVisualStyleBackColor = true;
            searchByIdButton.Click += BtnSearchById_Click;
            // 
            // resetSearchButton
            // 
            resetSearchButton.Location = new Point(114, 507);
            resetSearchButton.Margin = new Padding(4);
            resetSearchButton.Name = "resetSearchButton";
            resetSearchButton.Size = new Size(80, 33);
            resetSearchButton.TabIndex = 3;
            resetSearchButton.Text = "Сброс";
            resetSearchButton.UseVisualStyleBackColor = true;
            resetSearchButton.Click += BtnResetSearch_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.Location = new Point(340, 440);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(110, 23);
            label1.TabIndex = 14;
            label1.Text = "Поиск книг:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label2.Location = new Point(23, 440);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(115, 23);
            label2.TabIndex = 15;
            label2.Text = "Поиск по ID:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label3.Location = new Point(343, 573);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(190, 23);
            label3.TabIndex = 16;
            label3.Text = "Управление книгами:";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.jhoe;
            pictureBox1.Location = new Point(384, 670);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(373, 200);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 17;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // btnPrevPage
            // 
            btnPrevPage.Location = new Point(207, 399);
            btnPrevPage.Name = "btnPrevPage";
            btnPrevPage.Size = new Size(108, 30);
            btnPrevPage.TabIndex = 20;
            btnPrevPage.Text = "Предыдущая";
            btnPrevPage.UseVisualStyleBackColor = true;
            btnPrevPage.Click += BtnPrevPage_Click;
            // 
            // btnNextPage
            // 
            btnNextPage.Location = new Point(321, 399);
            btnNextPage.Name = "btnNextPage";
            btnNextPage.Size = new Size(118, 30);
            btnNextPage.TabIndex = 21;
            btnNextPage.Text = "Следующая";
            btnNextPage.UseVisualStyleBackColor = true;
            btnNextPage.Click += BtnNextPage_Click;
            // 
            // labelPageInfo
            // 
            labelPageInfo.Location = new Point(23, 399);
            labelPageInfo.Name = "labelPageInfo";
            labelPageInfo.Size = new Size(178, 20);
            labelPageInfo.TabIndex = 22;
            labelPageInfo.Text = "Страница 1";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 16F);
            label4.Location = new Point(6, 733);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(393, 37);
            label4.TabIndex = 18;
            label4.Text = "Какой же он крутой, я не могу";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(775, 653);
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
            Margin = new Padding(4);
            MaximumSize = new Size(793, 920);
            MinimumSize = new Size(793, 700);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Библиотека книг";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private PictureBox pictureBox1;
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPaginatedBooks();
        }

        private Label label4;
    }

}