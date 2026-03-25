using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.BlazoriseUI;

namespace ConferenceBooking.Blazor.Client.Pages;

public partial class Index
{
    protected override void Dispose(bool disposing)
    {
        PageLayout.ShowToolbar = true;
    }
}

