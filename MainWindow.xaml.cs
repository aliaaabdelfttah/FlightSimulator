using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FlightSimulator
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer simulationTimer;
        private double flightX = 100;
        private double flightY = 100;

        public MainWindow()
        {
            InitializeComponent();
            simulationTimer = new DispatcherTimer();
            simulationTimer.Interval = TimeSpan.FromMilliseconds(100);
            simulationTimer.Tick += SimulationTimer_Tick;

            DrawRadar();
        }

        private void StartSimulation_Click(object sender, RoutedEventArgs e)
        {
            RadarCanvas.Children.Clear();
            DrawRadar();
            simulationTimer.Start();
        }

        private void SimulationTimer_Tick(object sender, EventArgs e)
        {
            flightX += 3;
            flightY += 1;
            UpdateFlightPosition();
        }

        private void UpdateFlightPosition()
        {
            RadarCanvas.Children.Clear();
            DrawRadar();

            Ellipse flight = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.Red
            };
            Canvas.SetLeft(flight, flightX);
            Canvas.SetTop(flight, flightY);
            RadarCanvas.Children.Add(flight);

            InfoTextBlock.Text = $"Flight Position: X = {flightX}, Y = {flightY}";
        }

        private void DrawRadar()
        {
            int numRings = 4;
            double centerX = RadarCanvas.ActualWidth / 2;
            double centerY = RadarCanvas.ActualHeight / 2;
            double maxRadius = 200;

            for (int i = 1; i <= numRings; i++)
            {
                double radius = maxRadius * i / numRings;
                Ellipse ring = new Ellipse
                {
                    Width = radius * 2,
                    Height = radius * 2,
                    Stroke = Brushes.Green,
                    StrokeThickness = 1
                };
                Canvas.SetLeft(ring, centerX - radius);
                Canvas.SetTop(ring, centerY - radius);
                RadarCanvas.Children.Add(ring);
            }
        }
    }
}

using FlightSimulator.Data;
using FlightSimulator.Models;

private void SaveFlight_Click(object sender, RoutedEventArgs e)
{
    var flight = new Flight
    {
        FlightNumber = FlightNumberInput.Text,
        Altitude = int.Parse(AltitudeInput.Text),
        Speed = int.Parse(SpeedInput.Text),
        Status = ((ComboBoxItem)StatusCombo.SelectedItem)?.Content?.ToString()
    };

    using (var db = new AppDbContext())
    {
        db.Flights.Add(flight);
        db.SaveChanges();
    }

    InfoTextBlock.Text = $"Saved flight: {flight.FlightNumber}, Alt: {flight.Altitude} ft, Speed: {flight.Speed} knots";
}
private void LoadFlights_Click(object sender, RoutedEventArgs e)
{
    FlightsList.Items.Clear();
    using (var db = new AppDbContext())
    {
        var flights = db.Flights.ToList();
        foreach (var flight in flights)
        {
            FlightsList.Items.Add($"[{flight.Id}] {flight.FlightNumber} - {flight.Status} - {flight.Altitude}ft @ {flight.Speed}knots");
        }
    }
}


private double angle = 0;

private void SimulationTimer_Tick(object sender, EventArgs e)
{
    angle += 0.05;
    flightX = 200 + Math.Cos(angle) * 100;
    flightY = 200 + Math.Sin(angle) * 100;
    UpdateFlightPosition();
}

Line vertical = new Line
{
    X1 = centerX,
    Y1 = centerY - maxRadius,
    X2 = centerX,
    Y2 = centerY + maxRadius,
    Stroke = Brushes.DarkGreen,
    StrokeThickness = 1
};
RadarCanvas.Children.Add(vertical);

Line horizontal = new Line
{
    X1 = centerX - maxRadius,
    Y1 = centerY,
    X2 = centerX + maxRadius,
    Y2 = centerY,
    Stroke = Brushes.DarkGreen,
    StrokeThickness = 1
};
RadarCanvas.Children.Add(horizontal);

private void ExportToExcel()
{
    using (var db = new AppDbContext())
    {
        var flights = db.Flights.ToList();

        var wb = new ClosedXML.Excel.XLWorkbook();
        var ws = wb.Worksheets.Add("Flights");

        ws.Cell(1, 1).Value = "Flight Number";
        ws.Cell(1, 2).Value = "Altitude";
        ws.Cell(1, 3).Value = "Speed";
        ws.Cell(1, 4).Value = "Status";

        for (int i = 0; i < flights.Count; i++)
        {
            ws.Cell(i + 2, 1).Value = flights[i].FlightNumber;
            ws.Cell(i + 2, 2).Value = flights[i].Altitude;
            ws.Cell(i + 2, 3).Value = flights[i].Speed;
            ws.Cell(i + 2, 4).Value = flights[i].Status;
        }

        wb.SaveAs("FlightsReport.xlsx");
        MessageBox.Show("Exported to Excel successfully!");
    }
}

private void ExportToExcel_Click(object sender, RoutedEventArgs e)
{
    ExportToExcel();
}
