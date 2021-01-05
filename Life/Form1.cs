using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Life
{
    public partial class Form1 : Form
    {
        private LifeComponent lifeComponent;

        public Form1()
        {
            InitializeComponent();
        }

        public void InitLife()
        {
            lifeComponent = new LifeComponent();
            lifeComponent.OnLifeStop = OnLifeStop;
            lifeComponent.Parent = panelLife;
            lifeComponent.Dock = DockStyle.Fill;
            lifeComponent.Life.SetPointValue(5, 5, true);
            lifeComponent.Life.SetPointValue(5, 6, true);
            lifeComponent.Life.SetPointValue(6, 5, true);
            lifeComponent.Life.SetPointValue(8, 8, true);
            lifeComponent.Life.SetPointValue(7, 8, true);
            lifeComponent.Life.SetPointValue(8, 7, true);
            lifeComponent.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitLife();
            ChangeStateLife(false, "Остановлено");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lifeComponent.UpdateLife();
        }

        private void ChangeStateLife(bool alive, string message)
        {
            timer1.Enabled = alive;
            buttonStart.Enabled = !alive;
            buttonStop.Enabled = alive;
            label1.Text = message;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            ChangeStateLife(true, "Запущено");
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            ChangeStateLife(false, "Остановлено");
        }

        private void OnLifeStop(string message)
        {
            ChangeStateLife(false, "Остановлено: " + message);
        }
    }
}
