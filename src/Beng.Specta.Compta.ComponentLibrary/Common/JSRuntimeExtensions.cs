using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Beng.Specta.Compta.ComponentLibrary.Common
{
    public static class JSRuntimeExtensions
    {
        public static ValueTask<string> CalculateNodeHeightAsync(this IJSRuntime jsRuntime,
            ElementReference uiTextNode,
            bool useCache = false,
            int? minRows = null,
            int? maxRows = null) =>
                jsRuntime.InvokeAsync<string>("customComponentHandler.computeNodeHeight", uiTextNode, useCache, minRows, maxRows);

        public static ValueTask<object> RegisterWindowResizeAsync<T>(this IJSRuntime jsRuntime,
            int mobileWidth,
            DotNetObjectReference<T> dotNetObject) where T : class 
        {
            return jsRuntime.InvokeAsync<object>("customComponentHandler.registerWindowResizeListener", mobileWidth, dotNetObject);
        }

        public static ValueTask<object> UnregisterWindowResizeAsync<T>(this IJSRuntime jsRuntime,
            int mobileWidth,
            DotNetObjectReference<T> dotNetObject) where T : class
        {
            return jsRuntime.InvokeAsync<object>("customComponentHandler.unregisterWindowResizeListener", mobileWidth, dotNetObject);
        }

        public static ValueTask<object> RegisterClickOutsideAsync<T>(this IJSRuntime jsRuntime,
            ElementReference anchorRef,
            ElementReference contentRef,
            DotNetObjectReference<T> dotNetObject) where T : class
        {
            return jsRuntime.InvokeAsync<object>("customComponentHandler.registerOutsideClickListener", anchorRef, contentRef, dotNetObject); 
        }

        public static ValueTask<object> UnregisterClickOutsideAsync<T>(this IJSRuntime jsRuntime,
            ElementReference anchorRef,
            ElementReference contentRef,
            DotNetObjectReference<T> dotNetObject) where T : class
        {
            return jsRuntime.InvokeAsync<object>("customComponentHandler.unregisterOutsideClickListener", anchorRef, contentRef, dotNetObject); 
        }

        public static ValueTask<string> RemovePopper(this IJSRuntime jsRuntime) =>
                jsRuntime.InvokeAsync<string>("customComponentHandler.removePopper");

        public static ValueTask<object> DisableElement(this IJSRuntime jsRuntime) =>
                jsRuntime.InvokeAsync<object>("customComponentHandler.disableElement");

        public static ValueTask<object> ShowTippyTooltip(this IJSRuntime jsRuntime,
            ElementReference targetRef,
            ElementReference contentRef,
            PositionType position,
            TriggerType trigger,
            int duration,
            int showDelay,
            int hideDelay,
            bool hasArrow) =>
                jsRuntime.InvokeAsync<object>("customComponentHandler.showTippyTooltip",
                    targetRef,
                    contentRef,
                    position.ToDescriptionString(),
                    trigger.ToDescriptionString(),
                    duration,
                    showDelay,
                    hideDelay,
                    hasArrow);

        public static ValueTask<object> LaunchToast(this IJSRuntime jsRuntime,
            string type,
            string message,
            string title = null) =>
                jsRuntime.InvokeAsync<object>("customComponentHandler.launchToast",
                    type,
                    message,
                    title);

        public static ValueTask<string> HandlePopperInstanceAsync(this IJSRuntime jsRuntime,
            ElementReference anchorRef,
            ElementReference contentRef,
            ElementReference anchorWidthRef,
            PositionType position,
            string offset,
            bool isContentVisible,
            bool isPreventOverflow,
            bool isFixed,
            bool isBoundaryBody,
            bool isKeepAnchorWidth)
        {
            var positionString = position.ToDescriptionString();
            
            return jsRuntime.InvokeAsync<string>("customComponentHandler.handlePopperInstance",
                anchorRef,
                contentRef,
                anchorWidthRef,
                positionString,
                offset,
                isContentVisible,
                isPreventOverflow,
                isFixed,
                isBoundaryBody,
                isKeepAnchorWidth);
        }
    }
}