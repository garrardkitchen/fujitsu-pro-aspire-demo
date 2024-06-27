
# CD (GitHub)

_this does not create a workflow for you_

**Step 1** - Added workflow from template

**Step 2** - Added restore task:

```yaml
- name: Restore        
  run: |
    dotnet workload restore
    shell: pwsh
```

**Step 3** - Create secrets and variables for GH actions

This process automatically creates the GH secrets:

```
mkdir .github/workloads
azd pipeline config --provider github
```

**AZD_INITIAL_ENVIRONMENT_CONFIG** will contain external parameter secret values (e.g. env_MySecret)

