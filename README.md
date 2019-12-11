# iViewRenderer
Write view contents directly to the Http Response

How to use :

Inject IViewRenderer to your Controller.
Sample usage :

IViewRenderer _viewRenderer;
```c#
await _viweRenderer.RenderViewAsync(this, "viewName", Model);
This will render your view directly to the Http Response.
```

If you want to get a View that has a Model as string then :

```c#
await _viweRenderer.GetViewAsStringAsync<typeof(model>>(this, "viewName", model);
This will use your model to populate View and then return it as string.
```
