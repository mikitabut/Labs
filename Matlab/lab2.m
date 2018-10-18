string = 'The quick brown fox jumps over the lazy dog'
key = 1.5
n = round(sqrt(length(string)) / key);
string = [string, [repelem(' ', n - rem(length(string), n))]'];
reshape(string, n, [])