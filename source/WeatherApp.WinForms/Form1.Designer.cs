namespace WeatherApp.WinForms;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.tabControl = new System.Windows.Forms.TabControl();
        this.tabProfiles = new System.Windows.Forms.TabPage();
        this.lblActiveProfile = new System.Windows.Forms.Label();
        this.lstProfiles = new System.Windows.Forms.ListBox();
        this.grpProfileSettings = new System.Windows.Forms.GroupBox();
        this.lblHorizon = new System.Windows.Forms.Label();
        this.numHorizon = new System.Windows.Forms.NumericUpDown();
        this.lblTrainSize = new System.Windows.Forms.Label();
        this.numTrainSize = new System.Windows.Forms.NumericUpDown();
        this.lblSeriesLength = new System.Windows.Forms.Label();
        this.numSeriesLength = new System.Windows.Forms.NumericUpDown();
        this.lblWindowSize = new System.Windows.Forms.Label();
        this.numWindowSize = new System.Windows.Forms.NumericUpDown();
        this.lblProfileName = new System.Windows.Forms.Label();
        this.txtProfileName = new System.Windows.Forms.TextBox();
        this.lblModelType = new System.Windows.Forms.Label();
        this.cmbModelType = new System.Windows.Forms.ComboBox();
        this.btnDeleteProfile = new System.Windows.Forms.Button();
        this.btnSaveProfile = new System.Windows.Forms.Button();
        this.btnSetActive = new System.Windows.Forms.Button();
        this.btnAddProfile = new System.Windows.Forms.Button();
        this.tabTraining = new System.Windows.Forms.TabPage();
        this.lblTrainStatus = new System.Windows.Forms.Label();
        this.btnFineTune = new System.Windows.Forms.Button();
        this.btnTrain = new System.Windows.Forms.Button();
        this.dtTrainEnd = new System.Windows.Forms.DateTimePicker();
        this.dtTrainStart = new System.Windows.Forms.DateTimePicker();
        this.lblTrainEnd = new System.Windows.Forms.Label();
        this.lblTrainStart = new System.Windows.Forms.Label();
        this.lblTrainCity = new System.Windows.Forms.Label();
        this.txtTrainCity = new System.Windows.Forms.TextBox();
        this.lblTrainingDataInfo = new System.Windows.Forms.Label();
        this.gridTrainingData = new System.Windows.Forms.DataGridView();
        this.tabForecast = new System.Windows.Forms.TabPage();
        this.lblForecastStatus = new System.Windows.Forms.Label();
        this.gridForecast = new System.Windows.Forms.DataGridView();
        this.btnForecast = new System.Windows.Forms.Button();
        this.numForecastHorizon = new System.Windows.Forms.NumericUpDown();
        this.lblForecastHorizon = new System.Windows.Forms.Label();
        this.lblForecastCity = new System.Windows.Forms.Label();
        this.txtForecastCity = new System.Windows.Forms.TextBox();
        this.lblForecastDate = new System.Windows.Forms.Label();
        this.dtForecastDate = new System.Windows.Forms.DateTimePicker();
        this.tabControl.SuspendLayout();
        this.tabProfiles.SuspendLayout();
        this.grpProfileSettings.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.numHorizon)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.numTrainSize)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.numSeriesLength)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.numWindowSize)).BeginInit();
        this.tabTraining.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.gridTrainingData)).BeginInit();
        this.tabForecast.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.gridForecast)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.numForecastHorizon)).BeginInit();
        this.SuspendLayout();
        // 
        // tabControl
        // 
        this.tabControl.Controls.Add(this.tabProfiles);
        this.tabControl.Controls.Add(this.tabTraining);
        this.tabControl.Controls.Add(this.tabForecast);
        this.tabControl.Location = new System.Drawing.Point(12, 12);
        this.tabControl.Name = "tabControl";
        this.tabControl.SelectedIndex = 0;
        this.tabControl.Size = new System.Drawing.Size(960, 640);
        this.tabControl.TabIndex = 0;
        // 
        // tabProfiles
        // 
        this.tabProfiles.Controls.Add(this.lblActiveProfile);
        this.tabProfiles.Controls.Add(this.lstProfiles);
        this.tabProfiles.Controls.Add(this.grpProfileSettings);
        this.tabProfiles.Location = new System.Drawing.Point(4, 29);
        this.tabProfiles.Name = "tabProfiles";
        this.tabProfiles.Padding = new System.Windows.Forms.Padding(3);
        this.tabProfiles.Size = new System.Drawing.Size(952, 607);
        this.tabProfiles.TabIndex = 0;
        this.tabProfiles.Text = "Профили";
        this.tabProfiles.UseVisualStyleBackColor = true;
        // 
        // lblActiveProfile
        // 
        this.lblActiveProfile.AutoSize = true;
        this.lblActiveProfile.Location = new System.Drawing.Point(10, 520);
        this.lblActiveProfile.Name = "lblActiveProfile";
        this.lblActiveProfile.Size = new System.Drawing.Size(91, 20);
        this.lblActiveProfile.TabIndex = 2;
        this.lblActiveProfile.Text = "Активный: нет";
        // 
        // lstProfiles
        // 
        this.lstProfiles.FormattingEnabled = true;
        this.lstProfiles.ItemHeight = 20;
        this.lstProfiles.Location = new System.Drawing.Point(10, 10);
        this.lstProfiles.Name = "lstProfiles";
        this.lstProfiles.Size = new System.Drawing.Size(300, 484);
        this.lstProfiles.TabIndex = 0;
        this.lstProfiles.SelectedIndexChanged += new System.EventHandler(this.lstProfiles_SelectedIndexChanged);
        // 
        // grpProfileSettings
        // 
        this.grpProfileSettings.Controls.Add(this.lblHorizon);
        this.grpProfileSettings.Controls.Add(this.numHorizon);
        this.grpProfileSettings.Controls.Add(this.lblTrainSize);
        this.grpProfileSettings.Controls.Add(this.numTrainSize);
        this.grpProfileSettings.Controls.Add(this.lblSeriesLength);
        this.grpProfileSettings.Controls.Add(this.numSeriesLength);
        this.grpProfileSettings.Controls.Add(this.lblWindowSize);
        this.grpProfileSettings.Controls.Add(this.numWindowSize);
        this.grpProfileSettings.Controls.Add(this.lblProfileName);
        this.grpProfileSettings.Controls.Add(this.txtProfileName);
        this.grpProfileSettings.Controls.Add(this.lblModelType);
        this.grpProfileSettings.Controls.Add(this.cmbModelType);
        this.grpProfileSettings.Controls.Add(this.btnDeleteProfile);
        this.grpProfileSettings.Controls.Add(this.btnSaveProfile);
        this.grpProfileSettings.Controls.Add(this.btnSetActive);
        this.grpProfileSettings.Controls.Add(this.btnAddProfile);
        this.grpProfileSettings.Location = new System.Drawing.Point(330, 10);
        this.grpProfileSettings.Name = "grpProfileSettings";
        this.grpProfileSettings.Size = new System.Drawing.Size(600, 250);
        this.grpProfileSettings.TabIndex = 1;
        this.grpProfileSettings.TabStop = false;
        this.grpProfileSettings.Text = "Настройки профиля";
        // 
        // lblHorizon
        // 
        this.lblHorizon.AutoSize = true;
        this.lblHorizon.Location = new System.Drawing.Point(20, 200);
        this.lblHorizon.Name = "lblHorizon";
        this.lblHorizon.Size = new System.Drawing.Size(110, 20);
        this.lblHorizon.TabIndex = 11;
        this.lblHorizon.Text = "Горизонт (часы)";
        // 
        // numHorizon
        // 
        this.numHorizon.Location = new System.Drawing.Point(160, 198);
        this.numHorizon.Maximum = new decimal(new int[] { 336, 0, 0, 0 });
        this.numHorizon.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.numHorizon.Name = "numHorizon";
        this.numHorizon.Size = new System.Drawing.Size(120, 27);
        this.numHorizon.TabIndex = 5;
        this.numHorizon.Value = new decimal(new int[] { 24, 0, 0, 0 });
        // 
        // lblTrainSize
        // 
        this.lblTrainSize.AutoSize = true;
        this.lblTrainSize.Location = new System.Drawing.Point(20, 165);
        this.lblTrainSize.Name = "lblTrainSize";
        this.lblTrainSize.Size = new System.Drawing.Size(67, 20);
        this.lblTrainSize.TabIndex = 9;
        this.lblTrainSize.Text = "Размер обучения";
        // 
        // numTrainSize
        // 
        this.numTrainSize.Location = new System.Drawing.Point(160, 163);
        this.numTrainSize.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        this.numTrainSize.Name = "numTrainSize";
        this.numTrainSize.Size = new System.Drawing.Size(120, 27);
        this.numTrainSize.TabIndex = 4;
        // 
        // lblSeriesLength
        // 
        this.lblSeriesLength.AutoSize = true;
        this.lblSeriesLength.Location = new System.Drawing.Point(20, 130);
        this.lblSeriesLength.Name = "lblSeriesLength";
        this.lblSeriesLength.Size = new System.Drawing.Size(94, 20);
        this.lblSeriesLength.TabIndex = 7;
        this.lblSeriesLength.Text = "Длина ряда";
        // 
        // numSeriesLength
        // 
        this.numSeriesLength.Location = new System.Drawing.Point(160, 128);
        this.numSeriesLength.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        this.numSeriesLength.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
        this.numSeriesLength.Name = "numSeriesLength";
        this.numSeriesLength.Size = new System.Drawing.Size(120, 27);
        this.numSeriesLength.TabIndex = 3;
        this.numSeriesLength.Value = new decimal(new int[] { 48, 0, 0, 0 });
        // 
        // lblWindowSize
        // 
        this.lblWindowSize.AutoSize = true;
        this.lblWindowSize.Location = new System.Drawing.Point(20, 95);
        this.lblWindowSize.Name = "lblWindowSize";
        this.lblWindowSize.Size = new System.Drawing.Size(90, 20);
        this.lblWindowSize.TabIndex = 5;
        this.lblWindowSize.Text = "Размер окна";
        // 
        // numWindowSize
        // 
        this.numWindowSize.Location = new System.Drawing.Point(160, 93);
        this.numWindowSize.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        this.numWindowSize.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
        this.numWindowSize.Name = "numWindowSize";
        this.numWindowSize.Size = new System.Drawing.Size(120, 27);
        this.numWindowSize.TabIndex = 2;
        this.numWindowSize.Value = new decimal(new int[] { 24, 0, 0, 0 });
        // 
        // lblProfileName
        // 
        this.lblProfileName.AutoSize = true;
        this.lblProfileName.Location = new System.Drawing.Point(20, 30);
        this.lblProfileName.Name = "lblProfileName";
        this.lblProfileName.Size = new System.Drawing.Size(49, 20);
        this.lblProfileName.TabIndex = 3;
        this.lblProfileName.Text = "Название";
        // 
        // txtProfileName
        // 
        this.txtProfileName.Location = new System.Drawing.Point(160, 27);
        this.txtProfileName.Name = "txtProfileName";
        this.txtProfileName.Size = new System.Drawing.Size(200, 27);
        this.txtProfileName.TabIndex = 1;
        // 
        // lblModelType
        // 
        this.lblModelType.AutoSize = true;
        this.lblModelType.Location = new System.Drawing.Point(20, 60);
        this.lblModelType.Name = "lblModelType";
        this.lblModelType.Size = new System.Drawing.Size(83, 20);
        this.lblModelType.TabIndex = 4;
        this.lblModelType.Text = "Тип модели";
        // 
        // cmbModelType
        // 
        this.cmbModelType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbModelType.FormattingEnabled = true;
        this.cmbModelType.Location = new System.Drawing.Point(160, 57);
        this.cmbModelType.Name = "cmbModelType";
        this.cmbModelType.Size = new System.Drawing.Size(200, 28);
        this.cmbModelType.TabIndex = 2;
        // 
        // btnDeleteProfile
        // 
        this.btnDeleteProfile.Location = new System.Drawing.Point(390, 130);
        this.btnDeleteProfile.Name = "btnDeleteProfile";
        this.btnDeleteProfile.Size = new System.Drawing.Size(180, 29);
        this.btnDeleteProfile.TabIndex = 9;
        this.btnDeleteProfile.Text = "Удалить выбранный";
        this.btnDeleteProfile.UseVisualStyleBackColor = true;
        this.btnDeleteProfile.Click += new System.EventHandler(this.btnDeleteProfile_Click);
        // 
        // btnSaveProfile
        // 
        this.btnSaveProfile.Location = new System.Drawing.Point(390, 95);
        this.btnSaveProfile.Name = "btnSaveProfile";
        this.btnSaveProfile.Size = new System.Drawing.Size(180, 29);
        this.btnSaveProfile.TabIndex = 8;
        this.btnSaveProfile.Text = "Сохранить изменения";
        this.btnSaveProfile.UseVisualStyleBackColor = true;
        this.btnSaveProfile.Click += new System.EventHandler(this.btnSaveProfile_Click);
        // 
        // btnSetActive
        // 
        this.btnSetActive.Location = new System.Drawing.Point(390, 60);
        this.btnSetActive.Name = "btnSetActive";
        this.btnSetActive.Size = new System.Drawing.Size(180, 29);
        this.btnSetActive.TabIndex = 7;
        this.btnSetActive.Text = "Сделать активным";
        this.btnSetActive.UseVisualStyleBackColor = true;
        this.btnSetActive.Click += new System.EventHandler(this.btnSetActive_Click);
        // 
        // btnAddProfile
        // 
        this.btnAddProfile.Location = new System.Drawing.Point(390, 25);
        this.btnAddProfile.Name = "btnAddProfile";
        this.btnAddProfile.Size = new System.Drawing.Size(180, 29);
        this.btnAddProfile.TabIndex = 6;
        this.btnAddProfile.Text = "Добавить профиль";
        this.btnAddProfile.UseVisualStyleBackColor = true;
        this.btnAddProfile.Click += new System.EventHandler(this.btnAddProfile_Click);
        // 
        // tabTraining
        // 
        this.tabTraining.Controls.Add(this.lblTrainStatus);
        this.tabTraining.Controls.Add(this.gridTrainingData);
        this.tabTraining.Controls.Add(this.lblTrainingDataInfo);
        this.tabTraining.Controls.Add(this.btnFineTune);
        this.tabTraining.Controls.Add(this.btnTrain);
        this.tabTraining.Controls.Add(this.dtTrainEnd);
        this.tabTraining.Controls.Add(this.dtTrainStart);
        this.tabTraining.Controls.Add(this.lblTrainEnd);
        this.tabTraining.Controls.Add(this.lblTrainStart);
        this.tabTraining.Controls.Add(this.lblTrainCity);
        this.tabTraining.Controls.Add(this.txtTrainCity);
        this.tabTraining.Location = new System.Drawing.Point(4, 29);
        this.tabTraining.Name = "tabTraining";
        this.tabTraining.Padding = new System.Windows.Forms.Padding(3);
        this.tabTraining.Size = new System.Drawing.Size(952, 607);
        this.tabTraining.TabIndex = 1;
        this.tabTraining.Text = "Обучение";
        this.tabTraining.UseVisualStyleBackColor = true;
        // 
        // lblTrainStatus
        // 
        this.lblTrainStatus.AutoSize = true;
        this.lblTrainStatus.Location = new System.Drawing.Point(20, 170);
        this.lblTrainStatus.Name = "lblTrainStatus";
        this.lblTrainStatus.Size = new System.Drawing.Size(49, 20);
        this.lblTrainStatus.TabIndex = 8;
        this.lblTrainStatus.Text = "Статус";
        // 
        // lblTrainingDataInfo
        // 
        this.lblTrainingDataInfo.AutoSize = true;
        this.lblTrainingDataInfo.Location = new System.Drawing.Point(20, 205);
        this.lblTrainingDataInfo.Name = "lblTrainingDataInfo";
        this.lblTrainingDataInfo.Size = new System.Drawing.Size(175, 20);
        this.lblTrainingDataInfo.TabIndex = 9;
        this.lblTrainingDataInfo.Text = "Данные обучения: нет";
        // 
        // gridTrainingData
        // 
        this.gridTrainingData.AllowUserToAddRows = false;
        this.gridTrainingData.AllowUserToDeleteRows = false;
        this.gridTrainingData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        this.gridTrainingData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.gridTrainingData.Location = new System.Drawing.Point(20, 235);
        this.gridTrainingData.Name = "gridTrainingData";
        this.gridTrainingData.ReadOnly = true;
        this.gridTrainingData.RowHeadersWidth = 51;
        this.gridTrainingData.Size = new System.Drawing.Size(900, 340);
        this.gridTrainingData.TabIndex = 10;
        // 
        // btnFineTune
        // 
        this.btnFineTune.Location = new System.Drawing.Point(160, 125);
        this.btnFineTune.Name = "btnFineTune";
        this.btnFineTune.Size = new System.Drawing.Size(140, 29);
        this.btnFineTune.TabIndex = 7;
        this.btnFineTune.Text = "Дообучить";
        this.btnFineTune.UseVisualStyleBackColor = true;
        this.btnFineTune.Click += new System.EventHandler(this.btnFineTune_Click);
        // 
        // btnTrain
        // 
        this.btnTrain.Location = new System.Drawing.Point(20, 125);
        this.btnTrain.Name = "btnTrain";
        this.btnTrain.Size = new System.Drawing.Size(120, 29);
        this.btnTrain.TabIndex = 6;
        this.btnTrain.Text = "Обучить";
        this.btnTrain.UseVisualStyleBackColor = true;
        this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
        // 
        // dtTrainEnd
        // 
        this.dtTrainEnd.Location = new System.Drawing.Point(160, 85);
        this.dtTrainEnd.Name = "dtTrainEnd";
        this.dtTrainEnd.Size = new System.Drawing.Size(250, 27);
        this.dtTrainEnd.TabIndex = 5;
        // 
        // dtTrainStart
        // 
        this.dtTrainStart.Location = new System.Drawing.Point(160, 50);
        this.dtTrainStart.Name = "dtTrainStart";
        this.dtTrainStart.Size = new System.Drawing.Size(250, 27);
        this.dtTrainStart.TabIndex = 3;
        // 
        // lblTrainEnd
        // 
        this.lblTrainEnd.AutoSize = true;
        this.lblTrainEnd.Location = new System.Drawing.Point(20, 90);
        this.lblTrainEnd.Name = "lblTrainEnd";
        this.lblTrainEnd.Size = new System.Drawing.Size(65, 20);
        this.lblTrainEnd.TabIndex = 4;
        this.lblTrainEnd.Text = "Дата окончания";
        // 
        // lblTrainStart
        // 
        this.lblTrainStart.AutoSize = true;
        this.lblTrainStart.Location = new System.Drawing.Point(20, 55);
        this.lblTrainStart.Name = "lblTrainStart";
        this.lblTrainStart.Size = new System.Drawing.Size(71, 20);
        this.lblTrainStart.TabIndex = 2;
        this.lblTrainStart.Text = "Дата начала";
        // 
        // lblTrainCity
        // 
        this.lblTrainCity.AutoSize = true;
        this.lblTrainCity.Location = new System.Drawing.Point(20, 20);
        this.lblTrainCity.Name = "lblTrainCity";
        this.lblTrainCity.Size = new System.Drawing.Size(32, 20);
        this.lblTrainCity.TabIndex = 0;
        this.lblTrainCity.Text = "Город";
        // 
        // txtTrainCity
        // 
        this.txtTrainCity.Location = new System.Drawing.Point(160, 17);
        this.txtTrainCity.Name = "txtTrainCity";
        this.txtTrainCity.Size = new System.Drawing.Size(250, 27);
        this.txtTrainCity.TabIndex = 1;
        // 
        // tabForecast
        // 
        this.tabForecast.Controls.Add(this.lblForecastStatus);
        this.tabForecast.Controls.Add(this.gridForecast);
        this.tabForecast.Controls.Add(this.btnForecast);
        this.tabForecast.Controls.Add(this.dtForecastDate);
        this.tabForecast.Controls.Add(this.lblForecastDate);
        this.tabForecast.Controls.Add(this.numForecastHorizon);
        this.tabForecast.Controls.Add(this.lblForecastHorizon);
        this.tabForecast.Controls.Add(this.lblForecastCity);
        this.tabForecast.Controls.Add(this.txtForecastCity);
        this.tabForecast.Location = new System.Drawing.Point(4, 29);
        this.tabForecast.Name = "tabForecast";
        this.tabForecast.Padding = new System.Windows.Forms.Padding(3);
        this.tabForecast.Size = new System.Drawing.Size(952, 607);
        this.tabForecast.TabIndex = 2;
        this.tabForecast.Text = "Прогноз";
        this.tabForecast.UseVisualStyleBackColor = true;
        // 
        // lblForecastStatus
        // 
        this.lblForecastStatus.AutoSize = true;
        this.lblForecastStatus.Location = new System.Drawing.Point(20, 115);
        this.lblForecastStatus.Name = "lblForecastStatus";
        this.lblForecastStatus.Size = new System.Drawing.Size(49, 20);
        this.lblForecastStatus.TabIndex = 6;
        this.lblForecastStatus.Text = "Статус";
        // 
        // gridForecast
        // 
        this.gridForecast.AllowUserToAddRows = false;
        this.gridForecast.AllowUserToDeleteRows = false;
        this.gridForecast.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        this.gridForecast.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.gridForecast.Location = new System.Drawing.Point(20, 150);
        this.gridForecast.Name = "gridForecast";
        this.gridForecast.ReadOnly = true;
        this.gridForecast.RowHeadersWidth = 51;
        this.gridForecast.Size = new System.Drawing.Size(900, 420);
        this.gridForecast.TabIndex = 7;
        // 
        // btnForecast
        // 
        this.btnForecast.Location = new System.Drawing.Point(460, 50);
        this.btnForecast.Name = "btnForecast";
        this.btnForecast.Size = new System.Drawing.Size(120, 29);
        this.btnForecast.TabIndex = 5;
        this.btnForecast.Text = "Прогнозировать";
        this.btnForecast.UseVisualStyleBackColor = true;
        this.btnForecast.Click += new System.EventHandler(this.btnForecast_Click);
        // 
        // lblForecastDate
        // 
        this.lblForecastDate.AutoSize = true;
        this.lblForecastDate.Location = new System.Drawing.Point(300, 20);
        this.lblForecastDate.Name = "lblForecastDate";
        this.lblForecastDate.Size = new System.Drawing.Size(127, 20);
        this.lblForecastDate.TabIndex = 8;
        this.lblForecastDate.Text = "Дата прогноза";
        // 
        // dtForecastDate
        // 
        this.dtForecastDate.Location = new System.Drawing.Point(460, 17);
        this.dtForecastDate.Name = "dtForecastDate";
        this.dtForecastDate.Size = new System.Drawing.Size(200, 27);
        this.dtForecastDate.TabIndex = 4;
        // 
        // numForecastHorizon
        // 
        this.numForecastHorizon.Location = new System.Drawing.Point(160, 50);
        this.numForecastHorizon.Maximum = new decimal(new int[] { 336, 0, 0, 0 });
        this.numForecastHorizon.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.numForecastHorizon.Name = "numForecastHorizon";
        this.numForecastHorizon.Size = new System.Drawing.Size(120, 27);
        this.numForecastHorizon.TabIndex = 3;
        this.numForecastHorizon.Value = new decimal(new int[] { 24, 0, 0, 0 });
        // 
        // lblForecastHorizon
        // 
        this.lblForecastHorizon.AutoSize = true;
        this.lblForecastHorizon.Location = new System.Drawing.Point(20, 52);
        this.lblForecastHorizon.Name = "lblForecastHorizon";
        this.lblForecastHorizon.Size = new System.Drawing.Size(110, 20);
        this.lblForecastHorizon.TabIndex = 2;
        this.lblForecastHorizon.Text = "Горизонт (часы)";
        // 
        // lblForecastCity
        // 
        this.lblForecastCity.AutoSize = true;
        this.lblForecastCity.Location = new System.Drawing.Point(20, 20);
        this.lblForecastCity.Name = "lblForecastCity";
        this.lblForecastCity.Size = new System.Drawing.Size(32, 20);
        this.lblForecastCity.TabIndex = 0;
        this.lblForecastCity.Text = "Город";
        // 
        // txtForecastCity
        // 
        this.txtForecastCity.Location = new System.Drawing.Point(160, 17);
        this.txtForecastCity.Name = "txtForecastCity";
        this.txtForecastCity.Size = new System.Drawing.Size(250, 27);
        this.txtForecastCity.TabIndex = 1;
        // 
        // Form1
        // 
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(984, 661);
        this.Controls.Add(this.tabControl);
        this.Name = "Form1";
        this.Text = "Прогноз погоды (ML)";
        this.tabControl.ResumeLayout(false);
        this.tabProfiles.ResumeLayout(false);
        this.tabProfiles.PerformLayout();
        this.grpProfileSettings.ResumeLayout(false);
        this.grpProfileSettings.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.numHorizon)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.numTrainSize)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.numSeriesLength)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.numWindowSize)).EndInit();
        this.tabTraining.ResumeLayout(false);
        this.tabTraining.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.gridTrainingData)).EndInit();
        this.tabForecast.ResumeLayout(false);
        this.tabForecast.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.gridForecast)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.numForecastHorizon)).EndInit();
        this.ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.TabControl tabControl;
    private System.Windows.Forms.TabPage tabProfiles;
    private System.Windows.Forms.TabPage tabTraining;
    private System.Windows.Forms.TabPage tabForecast;
    private System.Windows.Forms.ListBox lstProfiles;
    private System.Windows.Forms.GroupBox grpProfileSettings;
    private System.Windows.Forms.Button btnDeleteProfile;
    private System.Windows.Forms.Button btnSaveProfile;
    private System.Windows.Forms.Button btnSetActive;
    private System.Windows.Forms.Button btnAddProfile;
    private System.Windows.Forms.Label lblProfileName;
    private System.Windows.Forms.TextBox txtProfileName;
    private System.Windows.Forms.Label lblModelType;
    private System.Windows.Forms.ComboBox cmbModelType;
    private System.Windows.Forms.Label lblWindowSize;
    private System.Windows.Forms.NumericUpDown numWindowSize;
    private System.Windows.Forms.Label lblSeriesLength;
    private System.Windows.Forms.NumericUpDown numSeriesLength;
    private System.Windows.Forms.Label lblTrainSize;
    private System.Windows.Forms.NumericUpDown numTrainSize;
    private System.Windows.Forms.Label lblHorizon;
    private System.Windows.Forms.NumericUpDown numHorizon;
    private System.Windows.Forms.Label lblActiveProfile;
    private System.Windows.Forms.Label lblTrainCity;
    private System.Windows.Forms.TextBox txtTrainCity;
    private System.Windows.Forms.DateTimePicker dtTrainStart;
    private System.Windows.Forms.DateTimePicker dtTrainEnd;
    private System.Windows.Forms.Label lblTrainStart;
    private System.Windows.Forms.Label lblTrainEnd;
    private System.Windows.Forms.Button btnTrain;
    private System.Windows.Forms.Button btnFineTune;
    private System.Windows.Forms.Label lblTrainStatus;
    private System.Windows.Forms.Label lblTrainingDataInfo;
    private System.Windows.Forms.DataGridView gridTrainingData;
    private System.Windows.Forms.TextBox txtForecastCity;
    private System.Windows.Forms.Label lblForecastCity;
    private System.Windows.Forms.Label lblForecastHorizon;
    private System.Windows.Forms.NumericUpDown numForecastHorizon;
    private System.Windows.Forms.Button btnForecast;
    private System.Windows.Forms.DataGridView gridForecast;
    private System.Windows.Forms.Label lblForecastStatus;
    private System.Windows.Forms.Label lblForecastDate;
    private System.Windows.Forms.DateTimePicker dtForecastDate;
}
