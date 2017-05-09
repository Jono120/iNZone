namespace InZoneKioskMailService {
	partial class InZoneMailService {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.tmrMain = new System.Timers.Timer();
			// 
			// tmrMain
			// 
			this.tmrMain.Enabled = false;
			this.tmrMain.Interval = 60000;
			this.tmrMain.Elapsed += new System.Timers.ElapsedEventHandler(this.tmrMain_Tick);
			// 
			// InZoneMailService
			// 
			this.ServiceName = "InZoneMailService";

		}

		#endregion

		private System.Timers.Timer tmrMain;
	}
}
