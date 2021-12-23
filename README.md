# Sitecore Send
Repository contains utility to submit custom form action from Sitecore to Send instance.

# Prerequisites
←`Sitecore Instance 10.2`

←`Sitecore helix basic Company` : You can choose implementations from https://sitecore.github.io/Helix.Examples/examples/helix-basic-company.html

←`Choice of IDE` : For this implementation Visual Studio 2019 is used.

## What's in this project?

← `App Folder`: Visual Studio Solution for Sitecore Send utility.

← `SitecorePackage Folder`: Contains Sitecore package which needs to be installed after your instance is loaded with Basic company helix implementation.

← `Sitecore.Send.util`: This project Contains `MoosendSubmitAction.cs` which has implementation to perform custom submit to Send Instance with data collected from Sitecore Form/Page.

## How to make it work / Steps for usage ?

← First make sure you have everything required.

← Clone Repository to your local and install sitecore package . This package will install `SubscribeMe` page under basic company site along with `Sitecore Send Demo` form under Forms.

← Make required Changes on `MoosendSubmitAction.cs` with your memeberlist id and Send Api key.

← Compile the project/solution and deploy to your instance.

← Visit Subscribe Me page from navigation.




