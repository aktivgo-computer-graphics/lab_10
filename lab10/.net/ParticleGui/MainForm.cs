using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;

using ZedGraph;

using ParticleGui.Tasks;
using ParticleSwarm;


namespace ParticleGui
{
	public partial class MainForm : Form
	{
		public MainForm ()
		{
			InitializeComponent ();

			CreateTasks ();
			InitGuiElelemts ();
		}

		void InitGuiElelemts ()
		{
			foreach (ITaskGui task in _tasks)
			{
				_tasksComboBox.Items.Add (task.Name);
			}

			if (_tasksComboBox.Items.Count != 0)
			{
				_tasksComboBox.SelectedIndex = 0;
			}

			SetGraphOptions ();
		}

		void CreateTasks ()
		{
			_tasks.Add (new TaskGuiX2 ());
			_tasks.Add (new TaskGuiShwefel ());
			_tasks.Add (new TaskGuiRastrigin ());
		}

		List<ITaskGui> _tasks = new List<ITaskGui> ();


		private void _createSwarm_Click (object sender, EventArgs e)
		{
			_errorProvider.Clear ();

			double currentVelocityRatio;
			try
			{
				currentVelocityRatio = Convert.ToDouble (_currentVelocityRatio.Text, 
					CultureInfo.InvariantCulture);
				
			}
			catch (FormatException)
			{
				_errorProvider.SetIconAlignment (_currentVelocityRatio,
					ErrorIconAlignment.MiddleLeft);

				_errorProvider.SetError (_currentVelocityRatio, "Неправильный формат числа");
				return;
			}


			double localBestRatio;
			try
			{
				localBestRatio = Convert.ToDouble (_localBestRatio.Text,
					CultureInfo.InvariantCulture);
			}
			catch (FormatException)
			{
				_errorProvider.SetIconAlignment (_localBestRatio,
					ErrorIconAlignment.MiddleLeft);

				_errorProvider.SetError (_localBestRatio, "Неправильный формат числа");
				return;
			}


			double globalBestRatio;
			try
			{
				globalBestRatio = Convert.ToDouble (_globalBestRatio.Text,
					CultureInfo.InvariantCulture);
			}
			catch (FormatException)
			{
				_errorProvider.SetIconAlignment (_globalBestRatio,
					ErrorIconAlignment.MiddleLeft);

				_errorProvider.SetError (_globalBestRatio, "Неправильный формат числа");
				return;
			}

			if ((localBestRatio + globalBestRatio) <= 4)
			{
				_errorProvider.SetIconAlignment (_globalBestRatio,
					ErrorIconAlignment.MiddleLeft);

				_errorProvider.SetError (_globalBestRatio, "Сумма значений 'Коэфф. собственного лучшего значения' и 'Коэфф. глобального лучшего значения' должна быть больше 4");
				return;
			}

			int dimension = (int)_dimension.Value;
			int swarmSize = (int)_swarmSize.Value;

			_currentTask = _tasks[_tasksComboBox.SelectedIndex];
			Task task = _currentTask.CreateTask (dimension);

			CreateSwarm (
				task,
				swarmSize,
				currentVelocityRatio,
				localBestRatio,
				globalBestRatio);
		}


		ITaskGui _currentTask = null;
		Swarm _swarm = null;


		void CreateSwarm (
			Task task,
			int swarmSize,
			double currentVelocityRatio,
			double localBestRatio,
			double globalBestRatio)
		{
			_swarm = new Swarm (task,
				swarmSize,
				currentVelocityRatio,
				localBestRatio,
				globalBestRatio
				);

			UpdateResults ();
		}

		void RunIterations (int iterationCount)
		{
			if (_swarm == null)
			{
				return;
			}

			Debug.Assert (_currentTask != null);

			for (int i = 0; i < iterationCount; i++)
			{
				_swarm.NextIteration ();
			}

			UpdateResults ();
		}


		public string ResultToString (Swarm swarm)
		{
			StringBuilder builder = new StringBuilder ();

			if (swarm.BestPosition != null)
			{
				for (int i = 0; i < swarm.Dimension; i++)
				{
					builder.AppendFormat ("X[{0}] = {1}\r\n", i, swarm.BestPosition[i]);
				}
			}
			return builder.ToString ();
		}


		void UpdateResults ()
		{
			Debug.Assert (_swarm != null);
			Debug.Assert (_currentTask != null);

			_bestPosition.Text = ResultToString (_swarm);
			_finalFuncTextBox.Text = _swarm.BestFinalFunc.ToString ();

			UpdateSwarmGraph ();
		}

		void SetGraphOptions ()
		{
			GraphPane pane = _swarmGraph.GraphPane;

			pane.IsFontsScaled = false;
			pane.XAxis.Title.IsVisible = false;
			pane.YAxis.Title.IsVisible = false;
			pane.Title.IsVisible = false;
			pane.Margin.All = 8;

			_swarmGraph.AxisChange ();
			_swarmGraph.Invalidate ();
		}


		void UpdateSwarmGraph ()
		{
			Debug.Assert (_swarm != null);
			Debug.Assert (_swarm.Dimension >= 2);

			PointPairList particlesList = new PointPairList ();
			foreach (Particle particle in _swarm.Particles)
			{
				particlesList.Add (particle.Position[0], particle.Position[1]);
			}

			GraphPane pane = _swarmGraph.GraphPane;
			pane.CurveList.Clear ();

			if (_swarm.Iteration == 0)
			{
				pane.XAxis.Scale.Min = _currentTask.MinValues[0];
				pane.XAxis.Scale.Max = _currentTask.MaxValues[0];

				pane.YAxis.Scale.Min = _currentTask.MinValues[1];
				pane.YAxis.Scale.Max = _currentTask.MaxValues[1];
			}

			LineItem particlesCurve = pane.AddCurve ("Все решения", particlesList,
				Color.Black, SymbolType.Default);

			particlesCurve.Line.IsVisible = false;
			particlesCurve.Symbol.Fill.Color = Color.Black;
			particlesCurve.Symbol.Fill.Type = FillType.Solid;
			particlesCurve.Symbol.Size = 2;

			_swarmGraph.AxisChange ();
			_swarmGraph.Invalidate ();
		}


		private void _1Iteration_Click (object sender, EventArgs e)
		{
			RunIterations (1);
		}

		private void _10Iterations_Click (object sender, EventArgs e)
		{
			RunIterations (10);
		}

		private void _100Iterations_Click (object sender, EventArgs e)
		{
			RunIterations (100);
		}

		private void _1000Iterations_Click (object sender, EventArgs e)
		{
			RunIterations (1000);
		}
	}
}
