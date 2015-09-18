using System;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Syncfusion.SfChart.XForms;

namespace Homezig
{
	public partial class testChartXaml : ContentPage
	{
		public testChartXaml ()
		{
			InitializeComponent ();


			DataModel2 dataModel = new DataModel2 ();

			cs.ItemsSource = dataModel.Precipitation;
			SplineSeriesH.ItemsSource = dataModel.HighTemperature;
			SplineSeriesL.ItemsSource = dataModel.LowTemperature;

		}
	}

	public class DataModel2
	{
		public ObservableCollection<ChartDataPoint> HighTemperature;

		public ObservableCollection<ChartDataPoint> LowTemperature;

		public ObservableCollection<ChartDataPoint> Precipitation;

		public DataModel2()
		{
			HighTemperature = new ObservableCollection<ChartDataPoint>();

			HighTemperature.Add(new ChartDataPoint("Jan", 42));
			HighTemperature.Add(new ChartDataPoint("Feb", 44));
			HighTemperature.Add(new ChartDataPoint("Mar", 53));
			HighTemperature.Add(new ChartDataPoint("Apr", 64));
			HighTemperature.Add(new ChartDataPoint("May", 75));
			HighTemperature.Add(new ChartDataPoint("Jun", 83));
			HighTemperature.Add(new ChartDataPoint("Jul", 87));
			HighTemperature.Add(new ChartDataPoint("Aug", 84));
			HighTemperature.Add(new ChartDataPoint("Sep", 78));
			HighTemperature.Add(new ChartDataPoint("Oct", 67));
			HighTemperature.Add(new ChartDataPoint("Nov", 55));
			HighTemperature.Add(new ChartDataPoint("Dec", 45));

			LowTemperature = new ObservableCollection<ChartDataPoint>();

			LowTemperature.Add(new ChartDataPoint("Jan", 27));
			LowTemperature.Add(new ChartDataPoint("Feb", 28));
			LowTemperature.Add(new ChartDataPoint("Mar", 35));
			LowTemperature.Add(new ChartDataPoint("Apr", 44));
			LowTemperature.Add(new ChartDataPoint("May", 54));
			LowTemperature.Add(new ChartDataPoint("Jun", 63));
			LowTemperature.Add(new ChartDataPoint("Jul", 68));
			LowTemperature.Add(new ChartDataPoint("Aug", 66));
			LowTemperature.Add(new ChartDataPoint("Sep", 59));
			LowTemperature.Add(new ChartDataPoint("Oct", 48));
			LowTemperature.Add(new ChartDataPoint("Nov", 38));
			LowTemperature.Add(new ChartDataPoint("Dec", 29));

			Precipitation = new ObservableCollection<ChartDataPoint>();

			Precipitation.Add(new ChartDataPoint("Jan", 3.03));
			Precipitation.Add(new ChartDataPoint("Feb", 2.48));
			Precipitation.Add(new ChartDataPoint("Mar", 3.23));
			Precipitation.Add(new ChartDataPoint("Apr", 3.15));
			Precipitation.Add(new ChartDataPoint("May", 4.13));
			Precipitation.Add(new ChartDataPoint("Jun", 3.23));
			Precipitation.Add(new ChartDataPoint("Jul", 4.13));
			Precipitation.Add(new ChartDataPoint("Aug", 4.88));
			Precipitation.Add(new ChartDataPoint("Sep", 3.82));
			Precipitation.Add(new ChartDataPoint("Oct", 3.07));
			Precipitation.Add(new ChartDataPoint("Nov", 2.83));
			Precipitation.Add(new ChartDataPoint("Dec", 2.8));
		}
	}
}

