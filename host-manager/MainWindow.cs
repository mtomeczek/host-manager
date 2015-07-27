using System;
using System.Windows;
using Gtk;
using System.Windows.Forms;
using System.IO;
using System.Net;
using hostmanager;
using GLib;
using System.Collections.Generic;

public partial class MainWindow: Gtk.Window
{
	
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		initHosts ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Gtk.Application.Quit ();
		a.RetVal = true;
	}

	protected void OnOpenActionActivated (object sender, EventArgs e)
	{		
		FileChooserDialog fileDialog = new FileChooserDialog ("Open file", null, FileChooserAction.Open,"Cancel", ResponseType.Cancel, "Select",ResponseType.Accept);
		fileDialog.ShowHidden = true;
		fileDialog.SetCurrentFolder("c:\\windows\\system32\\drivers\\etc\\");

		try{
			ResponseType result = (ResponseType)fileDialog.Run ();
			if(result == ResponseType.Accept){
				fillHosts(fileDialog.Filename);
			}
			else{
				
			}
			fileDialog.Destroy();				
		}
		catch(Exception ex)
		{
			
		}
	}
		
	private void initHosts(){
		TreeViewColumn colIp = new TreeViewColumn ();
		colIp.Title = "IP";
		CellRendererText colIpRenderer = new CellRendererText ();
		colIp.PackStart (colIpRenderer, true);
		colIp.AddAttribute (colIpRenderer, "Address", 0);
		colIp.SetCellDataFunc (colIpRenderer, new TreeCellDataFunc (renderHostIp));

		TreeViewColumn colHost = new TreeViewColumn ();
		colHost.Title = "Host";
		CellRendererText colHostRenderer = new CellRendererText ();
		colHost.PackStart (colHostRenderer, true);
		colHost.AddAttribute (colHostRenderer, "Host", 1);
		colHost.SetCellDataFunc (colHostRenderer, new TreeCellDataFunc (renderHostName));
		grdHosts.AppendColumn (colIp);
		grdHosts.AppendColumn (colHost);
	}

	private void fillHosts(string hostsFileName){
		List<HostDefinition> hosts= HostDefinition.readHosts (hostsFileName);
		ListStore hostsStore = new ListStore (typeof(HostDefinition));

		grdHosts.Model = hostsStore;
		foreach(HostDefinition host in hosts){
			hostsStore.AppendValues (host);
		}
			
	}

	private void renderHostIp (TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
	{
		HostDefinition host = (HostDefinition) model.GetValue (iter, 0);
		(cell as Gtk.CellRendererText).Text = host.Address.ToString();
	}

	private void renderHostName (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
	{
		HostDefinition host = (HostDefinition) model.GetValue (iter, 0);
		(cell as Gtk.CellRendererText).Text = host.Host;
	}
}
