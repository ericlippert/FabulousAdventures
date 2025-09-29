partial class LifeForm
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
        components = new System.ComponentModel.Container();
        display = new PictureBox();
        timer = new System.Windows.Forms.Timer(components);
        panel = new Panel();
        reportBox = new TextBox();
        speedLabel = new Label();
        slowerButton = new Label();
        fasterButton = new Label();
        loadButton = new Label();
        resetButton = new Label();
        playButton = new Label();
        title = new Label();
        ((System.ComponentModel.ISupportInitialize)display).BeginInit();
        panel.SuspendLayout();
        SuspendLayout();
        // 
        // display
        // 
        display.BackColor = SystemColors.ControlLightLight;
        display.Location = new Point(13, 12);
        display.Margin = new Padding(4, 3, 4, 3);
        display.Name = "display";
        display.Size = new Size(654, 433);
        display.TabIndex = 0;
        display.TabStop = false;
        display.Click += display_Click;
        display.MouseDown += display_MouseDown;
        display.MouseEnter += display_MouseEnter;
        display.MouseMove += display_MouseMove;
        display.MouseUp += display_MouseUp;
        // 
        // timer
        // 
        timer.Enabled = true;
        timer.Interval = 30;
        timer.Tick += timer_Tick;
        // 
        // panel
        // 
        panel.Controls.Add(reportBox);
        panel.Controls.Add(speedLabel);
        panel.Controls.Add(slowerButton);
        panel.Controls.Add(fasterButton);
        panel.Controls.Add(loadButton);
        panel.Controls.Add(resetButton);
        panel.Controls.Add(playButton);
        panel.Controls.Add(title);
        panel.Location = new Point(14, 451);
        panel.Margin = new Padding(4, 3, 4, 3);
        panel.Name = "panel";
        panel.Size = new Size(653, 81);
        panel.TabIndex = 1;
        // 
        // reportBox
        // 
        reportBox.AcceptsReturn = true;
        reportBox.Enabled = false;
        reportBox.Location = new Point(254, 3);
        reportBox.Margin = new Padding(4, 3, 4, 3);
        reportBox.Multiline = true;
        reportBox.Name = "reportBox";
        reportBox.Size = new Size(258, 77);
        reportBox.TabIndex = 8;
        // 
        // speedLabel
        // 
        speedLabel.Font = new Font("Lucida Console", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        speedLabel.Location = new Point(154, 31);
        speedLabel.Margin = new Padding(4, 0, 4, 0);
        speedLabel.Name = "speedLabel";
        speedLabel.Size = new Size(52, 33);
        speedLabel.TabIndex = 7;
        speedLabel.Text = "15";
        speedLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // slowerButton
        // 
        slowerButton.Font = new Font("Lucida Console", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        slowerButton.Location = new Point(126, 29);
        slowerButton.Margin = new Padding(4, 0, 4, 0);
        slowerButton.Name = "slowerButton";
        slowerButton.Size = new Size(34, 33);
        slowerButton.TabIndex = 6;
        slowerButton.Text = "⏬";
        slowerButton.TextAlign = ContentAlignment.MiddleCenter;
        slowerButton.Click += slowerButton_Click;
        // 
        // fasterButton
        // 
        fasterButton.Font = new Font("Lucida Console", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        fasterButton.Location = new Point(214, 29);
        fasterButton.Margin = new Padding(4, 0, 4, 0);
        fasterButton.Name = "fasterButton";
        fasterButton.Size = new Size(34, 33);
        fasterButton.TabIndex = 5;
        fasterButton.Text = "⏫";
        fasterButton.TextAlign = ContentAlignment.MiddleCenter;
        fasterButton.Click += fasterButton_Click;
        // 
        // loadButton
        // 
        loadButton.Font = new Font("Lucida Console", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        loadButton.Location = new Point(4, 29);
        loadButton.Margin = new Padding(4, 0, 4, 0);
        loadButton.Name = "loadButton";
        loadButton.Size = new Size(34, 33);
        loadButton.TabIndex = 4;
        loadButton.Text = "📁";
        loadButton.TextAlign = ContentAlignment.MiddleCenter;
        loadButton.Click += loadButton_Click;
        // 
        // resetButton
        // 
        resetButton.Font = new Font("Lucida Console", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        resetButton.Location = new Point(44, 29);
        resetButton.Margin = new Padding(4, 0, 4, 0);
        resetButton.Name = "resetButton";
        resetButton.Size = new Size(34, 33);
        resetButton.TabIndex = 3;
        resetButton.Text = "⏮︎";
        resetButton.TextAlign = ContentAlignment.MiddleCenter;
        resetButton.Click += resetButton_Click;
        // 
        // playButton
        // 
        playButton.Font = new Font("Lucida Console", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        playButton.Location = new Point(85, 29);
        playButton.Margin = new Padding(4, 0, 4, 0);
        playButton.Name = "playButton";
        playButton.Size = new Size(34, 33);
        playButton.TabIndex = 2;
        playButton.Text = "⏯︎";
        playButton.TextAlign = ContentAlignment.MiddleCenter;
        playButton.Click += playButton_Click;
        // 
        // title
        // 
        title.AutoSize = true;
        title.Font = new Font("Lucida Console", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        title.Location = new Point(6, 0);
        title.Margin = new Padding(4, 0, 4, 0);
        title.Name = "title";
        title.Size = new Size(167, 16);
        title.TabIndex = 0;
        title.Text = "Life is Fabulous";
        // 
        // LifeForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(681, 543);
        Controls.Add(panel);
        Controls.Add(display);
        KeyPreview = true;
        Margin = new Padding(4, 3, 4, 3);
        Name = "LifeForm";
        Text = "LifeForm";
        Load += LifeForm_Load;
        KeyDown += LifeForm_KeyDown;
        Resize += LifeForm_Resize;
        ((System.ComponentModel.ISupportInitialize)display).EndInit();
        panel.ResumeLayout(false);
        panel.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.PictureBox display;
    private System.Windows.Forms.Timer timer;
    private System.Windows.Forms.Panel panel;
    private System.Windows.Forms.Label title;
    private System.Windows.Forms.Label playButton;
    private System.Windows.Forms.Label resetButton;
    private System.Windows.Forms.Label loadButton;
    private System.Windows.Forms.Label speedLabel;
    private System.Windows.Forms.Label slowerButton;
    private System.Windows.Forms.Label fasterButton;
    private System.Windows.Forms.TextBox reportBox;
}
