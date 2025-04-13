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
