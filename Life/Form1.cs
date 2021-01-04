﻿using System;
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
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lifeComponent.UpdateLife();
        }
    }
}
