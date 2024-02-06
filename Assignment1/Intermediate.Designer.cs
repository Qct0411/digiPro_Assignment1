namespace Assignment1
{
    partial class Intermediate
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
            menuStrip1 = new MenuStrip();
            animateToolStripMenuItem = new ToolStripMenuItem();
            autoToolStripMenuItem = new ToolStripMenuItem();
            stopToolStripMenuItem = new ToolStripMenuItem();
            Previous = new Button();
            button2 = new Button();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { animateToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(382, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // animateToolStripMenuItem
            // 
            animateToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { autoToolStripMenuItem, stopToolStripMenuItem });
            animateToolStripMenuItem.Name = "animateToolStripMenuItem";
            animateToolStripMenuItem.Size = new Size(79, 24);
            animateToolStripMenuItem.Text = "Animate";
            // 
            // autoToolStripMenuItem
            // 
            autoToolStripMenuItem.Name = "autoToolStripMenuItem";
            autoToolStripMenuItem.Size = new Size(124, 26);
            autoToolStripMenuItem.Text = "Auto";
            autoToolStripMenuItem.Click += autoToolStripMenuItem_Click;
            // 
            // stopToolStripMenuItem
            // 
            stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            stopToolStripMenuItem.Size = new Size(124, 26);
            stopToolStripMenuItem.Text = "Stop";
            stopToolStripMenuItem.Click += stopToolStripMenuItem_Click;
            // 
            // Previous
            // 
            Previous.Location = new Point(43, 303);
            Previous.Name = "Previous";
            Previous.Size = new Size(94, 29);
            Previous.TabIndex = 1;
            Previous.Text = "Previous";
            Previous.UseVisualStyleBackColor = true;
            Previous.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(239, 303);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 2;
            button2.Text = "Next";
            button2.UseVisualStyleBackColor = true;
            // 
            // Intermediate
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(382, 353);
            Controls.Add(button2);
            Controls.Add(Previous);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Intermediate";
            Text = "Intermediate";
            Load += Intermediate_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem animateToolStripMenuItem;
        private ToolStripMenuItem autoToolStripMenuItem;
        private ToolStripMenuItem stopToolStripMenuItem;
        private Button Previous;
        private Button button2;
    }
}