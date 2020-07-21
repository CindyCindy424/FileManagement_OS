namespace file_system
{
    partial class FileEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
            #region Windows Form Designer generated code

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent()
        {
            this.textEditBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.textEditBox.Location = new System.Drawing.Point(20, 20);
            this.textEditBox.Name = "richTextBox1";
            this.textEditBox.Size = new System.Drawing.Size(550, 350);
            this.textEditBox.TabIndex = 0;
            this.textEditBox.Text = "";
            this.textEditBox.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // FileEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 390);
            this.Controls.Add(this.textEditBox);
            this.Name = "FileEditor";
            this.Text = "FileEditor";
            this.Load += new System.EventHandler(this.FileEditor_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FileEditor_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox textEditBox;
    }
}