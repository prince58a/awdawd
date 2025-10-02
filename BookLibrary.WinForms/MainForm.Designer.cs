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
        private Button btnRefresh;

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
            dataGridView1.Location = new Point(12, 12);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(760, 300);
            dataGridView1.TabIndex = 0;
            // 
            // add
            // 
            add.Location = new Point(300, 450);
            add.Name = "add";
            add.Size = new Size(120, 40);
            add.TabIndex = 8;
            add.Text = "Добавить";
            add.UseVisualStyleBackColor = true;
            add.Click += btnADD_Click;
            // 
            // del
            // 
            del.Location = new Point(540, 450);
            del.Name = "del";
            del.Size = new Size(120, 40);
            del.TabIndex = 10;
            del.Text = "Удалить";
            del.UseVisualStyleBackColor = true;
            del.Click += btnDEL_Click;
            // 
            // edit
            // 
            edit.Location = new Point(420, 450);
            edit.Name = "edit";
            edit.Size = new Size(120, 40);
            edit.TabIndex = 9;
            edit.Text = "Редактировать";
            edit.UseVisualStyleBackColor = true;
            edit.Click += btnEDIT_Click;
            // 
            // author
            // 
            author.Location = new Point(300, 350);
            author.Name = "author";
            author.Size = new Size(120, 35);
            author.TabIndex = 4;
            author.Text = "По автору";
            author.UseVisualStyleBackColor = true;
            author.Click += btnSORTAuthor_Click;
            // 
            // genre
            // 
            genre.Location = new Point(420, 350);
            genre.Name = "genre";
            genre.Size = new Size(120, 35);
            genre.TabIndex = 5;
            genre.Text = "По жанру";
            genre.UseVisualStyleBackColor = true;
            genre.Click += btnSORTGenre_Click;
            // 
            // year
            // 
            year.Location = new Point(540, 350);
            year.Name = "year";
            year.Size = new Size(120, 35);
            year.TabIndex = 6;
            year.Text = "С года";
            year.UseVisualStyleBackColor = true;
            year.Click += btnSORTYear_Click;
            // 
            // AuthorSearchComboBox
            // 
            AuthorSearchComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            AuthorSearchComboBox.FormattingEnabled = true;
            AuthorSearchComboBox.Location = new Point(300, 390);
            AuthorSearchComboBox.Name = "AuthorSearchComboBox";
            AuthorSearchComboBox.Size = new Size(120, 23);
            AuthorSearchComboBox.TabIndex = 11;
            // 
            // GenereSearchComboBox
            // 
            GenereSearchComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            GenereSearchComboBox.FormattingEnabled = true;
            GenereSearchComboBox.Location = new Point(420, 390);
            GenereSearchComboBox.Name = "GenereSearchComboBox";
            GenereSearchComboBox.Size = new Size(120, 23);
            GenereSearchComboBox.TabIndex = 12;
            // 
            // YearSearchComboBox
            // 
            YearSearchComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            YearSearchComboBox.FormattingEnabled = true;
            YearSearchComboBox.Location = new Point(540, 390);
            YearSearchComboBox.Name = "YearSearchComboBox";
            YearSearchComboBox.Size = new Size(120, 23);
            YearSearchComboBox.TabIndex = 13;
            // 
            // idSearchTextBox
            // 
            idSearchTextBox.Location = new Point(20, 350);
            idSearchTextBox.Name = "idSearchTextBox";
            idSearchTextBox.Size = new Size(150, 23);
            idSearchTextBox.TabIndex = 1;
            // 
            // searchByIdButton
            // 
            searchByIdButton.Location = new Point(20, 380);
            searchByIdButton.Name = "searchByIdButton";
            searchByIdButton.Size = new Size(70, 25);
            searchByIdButton.TabIndex = 2;
            searchByIdButton.Text = "Поиск";
            searchByIdButton.UseVisualStyleBackColor = true;
            searchByIdButton.Click += btnSearchById_Click;
            // 
            // resetSearchButton
            // 
            resetSearchButton.Location = new Point(100, 380);
            resetSearchButton.Name = "resetSearchButton";
            resetSearchButton.Size = new Size(70, 25);
            resetSearchButton.TabIndex = 3;
            resetSearchButton.Text = "Сброс";
            resetSearchButton.UseVisualStyleBackColor = true;
            resetSearchButton.Click += btnResetSearch_Click;

            btnRefresh = new Button();
            btnRefresh.Location = new Point(180, 450);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(120, 40);
            btnRefresh.TabIndex = 17;
            btnRefresh.Text = "Обновить";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.Location = new Point(300, 330);
            label1.Name = "label1";
            label1.Size = new Size(93, 19);
            label1.TabIndex = 14;
            label1.Text = "Поиск книг:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label2.Location = new Point(20, 330);
            label2.Name = "label2";
            label2.Size = new Size(97, 19);
            label2.TabIndex = 15;
            label2.Text = "Поиск по ID:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label3.Location = new Point(300, 430);
            label3.Name = "label3";
            label3.Size = new Size(162, 19);
            label3.TabIndex = 16;
            label3.Text = "Управление книгами:";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.jhoe;
            pictureBox1.Location = new Point(420, 496);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(352, 153);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 17;
            pictureBox1.TabStop = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 16F);
            label4.Location = new Point(40, 550);
            label4.Name = "label4";
            label4.Size = new Size(322, 30);
            label4.TabIndex = 18;
            label4.Text = "Какой же он крутой, я не могу";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 661);
            Controls.Add(label4);
            Controls.Add(pictureBox1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(resetSearchButton);
            Controls.Add(searchByIdButton);
            this.Controls.Add(btnRefresh);
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
            MaximumSize = new Size(800, 700);
            MinimumSize = new Size(800, 550);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Библиотека книг";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        private PictureBox pictureBox1;
        private Label label4;
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadBooks();
        }
    }

}