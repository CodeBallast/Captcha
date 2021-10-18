# Captcha
For those interested in tightening their security with Captcha in .NET Core I’ve created a highly configurable component. The main functionality is implemented in a project neutral class called Captcha. This class can easily be incorporated in a .NET Standard, WinForms, WPF, etc. component. My implementation incorporates the class in a razor component.   There are a lot of ways to configure the Captcha component. Amongst some of the most mentionable options are the size, the number of characters to display, the size of the font, the angles to display the characters, how many lines to use to pollute the bitmap, etc. 

Below you’ll see just one configuration of the Captcha component. 

```cs
@page "/"
@using CAPTCHA.Utils
@using CAPTCHA.Shared

<Captcha @ref="_captcha"
         RefreshButtonText=""
         CaseInsensitive="true"
         Width="580"
         Height="160"
         Pollution="15"
         MarginLeft="10"
         MarginTop="50"
         JumpY="45"
         Types="new ()
                {
                    StringRandomizerType.Number,
                    StringRandomizerType.Lower,
                    StringRandomizerType.Upper,
                    //StringRandomizerType.LowerDanish,
                    //StringRandomizerType.UpperDanish,
                    //StringRandomizerType.Special,
                    //StringRandomizerType.Custom
                }"
         CharacterSize="12"
         CustomCharacters="@(",._?'*")"
         MinFontSize="25"
         MaxFontSize="35"
         MinAngle="-20"
         MaxAngle="20">
</Captcha>

<br>
<button class="btn btn-primary" width="100%" @onclick="@Verify">Verify</button>
<br>

@if (_verifying)
{
    if (_correctInput)
    {
        <div>CORRECT</div>
    }
    else
    {
        <div>INCORRECT</div>
    }
}

@code
{
    private Shared.Captcha _captcha;
    private bool _correctInput;
    private bool _verifying = false;

    private async Task Verify()
    {
        _verifying = true;
        _correctInput = _captcha.Verify();
        StateHasChanged();
        await Task.CompletedTask;
    }
}
```
