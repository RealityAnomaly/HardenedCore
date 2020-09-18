# DRX Editor 3
## Introduction
DRX Editor 3 is the third iteration of the DRX Editor application developed by me, rebuilt from the ground up for the UWP platform. It is licensed under the MIT License like the rest of the HardenedCore code.

### Secure by design
DRX Editor 3 is designed from the ground up to be secure. It utilises several encryption methods documented in Features, and will enforce encryption and security levels depending on the flags you choose, ensuring no accidental data leakage. DRX Editor can leverage physical security tokens in the form of smart cards (YubiKey 4 is highly reccomended) and hardware based security such as TPMs on supported systems. Additionally with HSM enabled (on supported configurations) DRX Editor will apply additional measures to prevent leakage such as screenshot blanking and display link encryption via HDCP.

## Features
- Many encryption methods (password, certificate, etc)
- High-security mode available on select hardware configurations
- Metadata support, e.g. date, VREL and security level
- Categorisation of documents by flag
- Lightweight, compact and resilient format utilising MessagePack
- Document share functionality with automatic redactions

![](https://i.imgur.com/zKF0Wuq.png)
