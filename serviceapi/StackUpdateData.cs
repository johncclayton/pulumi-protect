﻿namespace pprot.serviceapi;

public record StackUpdateData(StackUpdateInfo Info, int Version, int LatestVersion, string UpdateID);

public record StackUpdateResourceChanges(int Create);
