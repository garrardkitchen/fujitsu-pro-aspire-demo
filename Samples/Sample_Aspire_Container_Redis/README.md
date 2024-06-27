# Demonstrating

Different categories of caching and the use if a container.  Container also is deleted once debug session finishes.

# Notes

- Output cache

  Open apiservice endpoint first and refresh and observe json changing on each refresh
  
  Now open blazor app and refresh weather page and observe values changing every 5 seconds

- Distributed cache
  
  Press counter a few times then nav away, and return.  Counter is remembered.

  Now exec into container to retrieve value
  

Get counter value:

```
redis-cli
hget counter data
```