#
# Common functions
#

function Get-RandomKey {
    $length=32
    $randomString=-join (((33..126)) * 100 | Get-Random -Count $length | %{[char]$_})
    $bytes=[System.Text.Encoding]::Unicode.GetBytes($randomString)
    $key=[Convert]::ToBase64String($bytes)
    return $key
}

function Get-RandomLowercaseAndNumbers {
    Param([int]$length)

    # lowercase letters and numbers only
    $randomString=-join (((48..57)+(97..122)) * 100 | Get-Random -Count $length | %{[char]$_})
    return $randomString
}
function Get-RandomNumbers {
    Param([int]$length)

    $randomString=-join (((48..57)) * 100 | Get-Random -Count $length | %{[char]$_})
    return $randomString
}
