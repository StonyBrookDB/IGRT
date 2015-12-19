using System;
//using System.Text;
using System.Windows;
//using VMS.TPS.Common.Model.API;
//using VMS.TPS.Common.Model.Types;
//using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using System.IO;
using System.Text.RegularExpressions;

namespace VMS.TPS {
	class Script {
		public Script() {}
		
		public void print_voxels(Image image, int slice_number, string file_name, double z_step){
			int[, ] voxelPlane = new int[image.XSize, image.YSize];
			int current_slice=0;
			System.IO.TextWriter writer = new System.IO.StreamWriter("u:\\sample_script\\slices\\" + file_name);
			for (int z = 0; z < image.ZSize; z++) {			
				image.GetVoxels(z, voxelPlane);
				current_slice++;
				
				if (current_slice==slice_number){
					MessageBox.Show("slice Z was "+slice_number*z_step);
					for (int x = 0; x < image.XSize; x++) {
						for (int y = 0; y < image.YSize; y++) {
							int imageValue = voxelPlane[x, y];
								{
									writer.Write(imageValue +  ",");
								}
						}

					}
				}
			}
			writer.Close();
		}
		
		public void get_2d_slice(Series myseries, double z_value_isocenter, string file_name){
			MessageBox.Show("Seris is: "+myseries.Id);
			Image volume_image=null;
			foreach(Image temp_image in myseries.Images) {
			   if (!double.IsNaN(temp_image.ZDirection.x )){
				   volume_image=temp_image;
					MessageBox.Show("Zsize is: "+temp_image.ZSize);
				   break;
			   }   					
			}	
						
			//MessageBox.Show("Image_id is 3d " + volume_image.Id);
			//MessageBox.Show("Slice value (zres) " + volume_image.ZRes);
			double z_start=volume_image.Origin.z;
			double z_step=volume_image.ZRes;
			int slice_number=(int)Math.Ceiling((z_value_isocenter-z_start)/z_step);
			MessageBox.Show("slice number wanated "+slice_number);
			print_voxels(volume_image, slice_number, file_name,z_step);

		}
		
		public void Execute(ScriptContext context) {
			//Image volume_ct_image;			
			DateTime maxDate = new DateTime();
			maxDate=DateTime.MinValue;
			Series maxSeries=null;
			Series ctSeries=null;

			//System.IO.TextWriter writer = new System.IO.StreamWriter("u:\\sample_script\\max_vals.txt");
            string CtDeviceId = "GE CT RadOnc";
			StringBuilder sb = new StringBuilder();
			Dictionary <DateTime, string > seriesDateDict = new Dictionary < DateTime, string > ();
            double isoCenterX = 1000000.0;
            double isoCenterY = 1000000.0;
            double isoCenterZ = 1000000.0;
            
            
			PlanSetup myplan=context.PlanSetup;
            IEnumerable<Beam> mybeams = myplan.Beams;
            foreach (Beam element in mybeams) {
                    isoCenterX = element.IsocenterPosition.x;
                    isoCenterY = element.IsocenterPosition.y;
                    isoCenterZ = element.IsocenterPosition.z;
                    break;

            }
			
            //MessageBox.Show("isoCeneter: " + " X: " + isoCenterX.ToString() + " Y: " +   isoCenterY.ToString() + " Z: " + isoCenterZ.ToString());
			foreach(Study patientStudy in context.Patient.Studies) {
				foreach(Series patientSeries in patientStudy.Series) {
				int modality_value=(int)patientSeries.Modality;
				if (modality_value==0){ // 0  is CT
					if (patientSeries.ImagingDeviceId == CtDeviceId && !(String.IsNullOrEmpty(patientSeries.Comment))) {
						ctSeries=patientSeries;

					}
					
					else{
						if (DateTime.Compare(patientSeries.HistoryDateTime , maxDate) > 0){
							maxDate=patientSeries.HistoryDateTime;
							maxSeries=patientSeries;
							//writer.WriteLine("Current max is "+patientSeries.Id);
							//writer.WriteLine("Current max_date is "+patientSeries.HistoryDateTime);
							
						}
				
					} //works
				}

			}

			//Referencing CT and CBCT images
				//MessageBox.Show("CT image volume is: "+volume_ct_image.Id);
				//MessageBox.Show("Latest series is: "+maxSeries.Id);
				//MessageBox.Show("Latest date is: "+maxSeries.HistoryDateTime);
				//MessageBox.Show("Latest UID is: "+maxSeries.UID);
				//writer.WriteLine("I'm last line");
				//writer.Close();
				get_2d_slice(ctSeries,isoCenterZ, "ct_slice.txt");
				get_2d_slice(maxSeries,isoCenterZ, "cbct_slice.txt");
				//MessageBox.Show("Latest ToString is: "+maxSeries.ToString);
				}

			}
	} 
}