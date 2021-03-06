﻿#region Copyright (C) 2009-2010 Simon Allaeys

/*
    Copyright (C) 2009-2010 Simon Allaeys
 
    This file is part of AppStract

    AppStract is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    AppStract is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with AppStract.  If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

namespace AppStract.Manager.Packaging
{
  /// <summary>
  /// Stores the state of the current running wizard.
  /// </summary>
  public class PreConfigurationState
  {

    #region Properties

    /// <summary>
    /// Gets or sets the executable of the new application's installer.
    /// </summary>
    public string InstallerExecutable { get; set; }

    /// <summary>
    /// Gets or sets the destination where the installer should output the application.
    /// </summary>
    public string InstallerOutputDestination { get; set; }

    /// <summary>
    /// Gets or sets whether the engine configuration utility must be shown before launching the installer.
    /// </summary>
    public bool ShowEngineConfigurationUtility { get; set; }

    /// <summary>
    /// Gets or sets whether the application must be autostarted after packaging.
    /// </summary>
    public bool Autostart { get; set; }

    #endregion

    #region Constructors

    public PreConfigurationState()
    {
      InstallerExecutable = "";
      InstallerOutputDestination = "";
      ShowEngineConfigurationUtility = true;
    }

    #endregion

  }
}
