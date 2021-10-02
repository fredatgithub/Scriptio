///////////////////////////////////////////////////////////////////////////////////////////////
//  Scriptio - Script SQL Server 2005 objects
//  Copyright (C) 2005 Bill Graziano

//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.

//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.

//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
///////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Deployment.Application;
using System.Reflection;
using System.Windows.Forms;

namespace Scriptio
{
  public partial class AboutForm : Form
  {
    public AboutForm()
    {
      InitializeComponent();

      lblAssemblyVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString().Trim();

      lblClickOnceVersion.Text = (ApplicationDeployment.IsNetworkDeployed ? " " + ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString().Trim() : string.Empty).Trim();

      if (ApplicationDeployment.IsNetworkDeployed)
      {
        btnUpdates.Enabled = true;
      }
      else
      {
        btnUpdates.Enabled = false;
      }
    }

    private void BtnClose_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void BtnUpdates_Click(object sender, EventArgs e)
    {
      if (ApplicationDeployment.IsNetworkDeployed)
      {
        Cursor currentCursor = Cursor.Current;
        Cursor.Current = Cursors.WaitCursor;
        try
        {
          if (ApplicationDeployment.CurrentDeployment.CheckForUpdate())
          {
            Cursor.Current = currentCursor;
            if (MessageBox.Show(
                "An updated version is available. Would you like to update now?",
                "Update Found", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
              Cursor.Current = Cursors.WaitCursor;
              ApplicationDeployment.CurrentDeployment.Update();
              Cursor.Current = currentCursor;
              MessageBox.Show("Update downloaded. Scriptio will now restart.");
              Application.Restart();
            }
          }
          else
          {
            Cursor.Current = currentCursor;
            MessageBox.Show("No updates available at this time.");
          }
        }
        finally
        {
          Cursor.Current = currentCursor;
        }
      }
    }
  }
}