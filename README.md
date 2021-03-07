# My Hubitat Automation

[![Deploy My Hubitat to Azure Functios](https://github.com/myty/my-hubitat/actions/workflows/windows-dotnet-functionapp-on-azure.yml/badge.svg)](https://github.com/myty/my-hubitat/actions/workflows/windows-dotnet-functionapp-on-azure.yml)

For years I had all of our outside z-wave lights hooked up to a Wink hub.
I had my eye on getting a Hubitat for a while mostly because of it's hackability and the fact that it is self hosted.
I recently made the switch and have been enjoying it's abilty to be customized.

One issue we occasionaly have is when one of the outside lights either get accidentaly or intentionally turned off.
The problem is that the light will stay off for the remainder of the night.
I wanted to see if there was a way to automate turning any of those light swithces back on and this is what I came up with.
It's definitely a work in progress, but was a fun and it works as expected.

## Overview

I decided to use Azure Functions, but any of the serverless platforms would work just as well.
My language and platform of choice is .Net Core and C#.
I also implemented a Github Action that will deploy the azure function whenever there is a new commit to the main branch.

## Future Work

I want to add testing and a guide to help others get setup if they'd like to do this themselves. Keep checking back in for more updates or star this repo to get notifications.