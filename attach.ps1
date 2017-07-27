$GitHubAuthToken = $env:GITHUB_AUTH_TOKEN
$ReleaseName = $env:APPVEYOR_REPO_TAG_NAME

$repo = "blairconrad/FakeItEasy"
$releasesUrl = "https://api.github.com/repos/$repo/releases"
$headers = @{                                                                  
    "Authorization" = "Bearer $GitHubAuthToken"
    "Content-type"  = "application/json"                                  
}

$releases = Invoke-RestMethod -Uri $releasesUrl -Headers $headers -Method GET
$release = $releases | Where-Object { $_.name -eq $ReleaseName }


if (! $release) {
    throw "Can not find release '$ReleaseName'"
}

$uploadsUrl = "https://uploads.github.com/repos/$repo/releases/$($release.id)/assets?name="
$headers["Content-type"] = "application/octet-stream"

$artifacts = Get-ChildItem -Path "artifacts/output/*.nupkg"
$artifacts | ForEach-Object { 
    Write-Output "Uploading $($_.Name) to release $ReleaseName"
    $asset = Invoke-RestMethod -Uri ($uploadsUrl + $_.Name) -Headers $headers -Method POST -InFile $_
    Write-Output "Uploaded  $($asset.name) ($($asset.size) bytes)"
}
