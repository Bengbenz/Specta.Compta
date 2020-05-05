
//import { createPopper } from '@popperjs/core';

let popperInstance = null;
let tippyInstance = null;

window.customComponentHandler = {
    registerOutsideClickListener: function (anchor, content, dotnetHelper) {
        window.addEventListener("click", e => handleDocumentClick(e, anchor, content, dotnetHelper), false);
    },
    unregisterOutsideClickListener: function (anchor, content, dotnetHelper) {
        window.removeEventListener("click", e => handleDocumentClick(e, anchor, content, dotnetHelper), false);
    },
    registerWindowResizeListener: function (mobileWidth, dotnetHelper) {
        window.addEventListener("resize", () => updateSidebarState(mobileWidth, dotnetHelper), false);
    },
    unregisterWindowResizeListener: function (mobileWidth, dotnetHelper) {
        window.removeEventListener("resize", () => updateSidebarState(mobileWidth, dotnetHelper), false);
    },
    handlePopperInstance: function (anchor,
        content,
        anchorWidthContainer,
        position,
        offset,
        isVisible,
        isPreventOverflow,
        isFixed,
        isBoundaryBody,
        isKeepAnchorWidth) {

        if (this.popperInstance) {
            removePopper();
        }

        if (!isVisible) {
            return;
        }

        return initPopper(anchor,
            content,
            anchorWidthContainer,
            position,
            offset,
            isPreventOverflow,
            isFixed,
            isBoundaryBody,
            isKeepAnchorWidth);
    },
    removePopper: function () {
        if (!this.popperInstance)
        {
            return;
        }
        
        this.popperInstance.destroy();
        this.popperInstance = null;
    },
    computeNodeHeight: function (uiTextNode,
        useCache = false,
        minRows = null,
        maxRows = null) {

        if (!hiddenTextarea) {
          hiddenTextarea = document.createElement('textarea')
          document.body.appendChild(hiddenTextarea)
        }
        
        // Fix wrap="off" issue
        // https://github.com/ant-design/ant-design/issues/6577
        if (uiTextNode.getAttribute('wrap')) {
          hiddenTextarea.setAttribute('wrap', uiTextNode.getAttribute('wrap'))
        } else {
          hiddenTextarea.removeAttribute('wrap')
        }
      
        // Copy all CSS properties that have an impact on the height of the content in
        // the textbox
        const { paddingSize, borderSize, boxSizing, sizingStyle } = calculateNodeStyling(
          uiTextNode,
          useCache,
        )
      
        // Need to have the overflow attribute to hide the scrollbar otherwise
        // text-lines will not calculated properly as the shadow will technically be
        // narrower for content
        hiddenTextarea.setAttribute('style', `${sizingStyle};${HIDDEN_TEXTAREA_STYLE}`)
        hiddenTextarea.value = uiTextNode.value || uiTextNode.placeholder || ''
      
        let minHeight = Number.MIN_SAFE_INTEGER
        let maxHeight = Number.MAX_SAFE_INTEGER
        let height = hiddenTextarea.scrollHeight
        let overflowY
      
        if (boxSizing === 'border-box') {
          // border-box: add border, since height = content + padding + border
          height = height + borderSize
        } else if (boxSizing === 'content-box') {
          // remove padding, since height = content
          height = height - paddingSize
        }
      
        if (minRows !== null || maxRows !== null) {
          // measure height of a textarea with a single row
          hiddenTextarea.value = ' '
          const singleRowHeight = hiddenTextarea.scrollHeight - paddingSize
          if (minRows !== null) {
            minHeight = singleRowHeight * minRows
            if (boxSizing === 'border-box') {
              minHeight = minHeight + paddingSize + borderSize
            }
            height = Math.max(minHeight, height)
          }
          if (maxRows !== null) {
            maxHeight = singleRowHeight * maxRows
            if (boxSizing === 'border-box') {
              maxHeight = maxHeight + paddingSize + borderSize
            }
            overflowY = height > maxHeight ? '' : 'hidden'
            height = Math.min(maxHeight, height)
          }
        }
      
        if (!maxRows) {
          overflowY = 'hidden'
        }
      
        const textareaStyles = {
          height: `${height}px`,
          minHeight: `${minHeight}px`,
          maxHeight: `${maxHeight}px`,
          overflowY,
        }
        // We modify DOM directly instead of using reactivity because the whole adjustHeight method takes place
        // each time the value of textarea is modified, so there's no real need in an additional layer of reactivity.
        // The operation is basically reactive though implicitly.
        Object.assign(uiTextNode.style, textareaStyles)
        return textareaStyles
    }
}

function handleDocumentClick (event, anchor, content, dotnetHelper) {

    let el = event.target;
    const clickedElements = []; // Array because dropdowns can be nested.
    // TODO Make DOM walk-over global, so that each dropdown doesn't have to do it.
    while (el) {
        clickedElements.push(el);
        el = el.parentNode;
    }
    const isCurrentDropdownClicked = clickedElements.includes(anchor) || clickedElements.includes(content);
    if (isCurrentDropdownClicked) {
        return;
    }

    if (dotnetHelper) dotnetHelper.invokeMethodAsync("OnClickOutside");
}

function scrollWidth () {
    const div = document.createElement('div');

    div.style.overflowY = 'scroll';
    div.style.width = '50px';
    div.style.height = '50px';
    div.style.visibility = 'hidden';

    document.body.appendChild(div);
    const scrollWidth = div.offsetWidth - div.clientWidth;
    document.body.removeChild(div);
    return scrollWidth;
}

function checkIsDesktop (mobileWidth) {
    return window.matchMedia(`(min-width: ${mobileWidth}px)`).matches
}

function updateSidebarState (mobileWidth, dotnetHelper) {
    if (checkIsDesktop(mobileWidth)) {
        if (dotnetHelper) dotnetHelper.invokeMethodAsync("DisableMinimizedState");
    }
    else
    {
      if (dotnetHelper) dotnetHelper.invokeMethodAsync("EnableMinimizedState");
    }
    disableElement();
}

function disableElement ()
{
    var el = document.getElementById("blazor-license-warning");
    if (el)
    {
        el.style = "display: none";
        console.log("blazor disabled");
    }
}

function initPopper (anchor,
    content,
    anchorWidthContainer,
    position,
    offset,
    isPreventOverflow,
    isFixed,
    isboundaryBody,
    isKeepAnchorWidth) {

    var offsetSplitted = offset.split(',');
    var skidding = new Number(offsetSplitted[0] || 0);
    var distance = new Number(offsetSplitted[1] || 0);
    const options = {
        placement: position || 'bottom',
        strategy: isFixed ? 'fixed' : 'absolute',
        modifiers: [
            {
                name: 'offset',
                options: {
                    offset: () => [skidding, distance],
                },
            },
            {
                name: 'preventOverflow',
                options: {
                    boundary: isboundaryBody ? window : document,
                    tether: isPreventOverflow
                },
            }
        ],
    }

    this.popperInstance = Popper.createPopper(
        anchor,
        content,
        options);

    // temporary solution
    return updateAnchorWidth(isKeepAnchorWidth, anchor, anchorWidthContainer);
}

function updateAnchorWidth (isKeepAnchorWidth, anchor, anchorWidthContainer) {
    let anchorWidth = null;
    if (isKeepAnchorWidth) {
        anchorWidth = anchor.offsetWidth;
        if (anchorWidthContainer && anchorWidthContainer.scrollHeight > anchorWidthContainer.clientHeight) {
            anchorWidth = anchor.offsetWidth - scrollWidth()
        }
    }

    if (this.popperInstance) {
        this.popperInstance.update()
    }
    return anchorWidth;
}

// Adopted from https://github.com/andreypopp/react-textarea-autosize/

const HIDDEN_TEXTAREA_STYLE = `
  min-height:0 !important;
  max-height:none !important;
  height:0 !important;
  visibility:hidden !important;
  overflow:hidden !important;
  position:absolute !important;
  z-index:-1000 !important;
  top:0 !important;
  right:0 !important
`

const SIZING_STYLE = [
  'letter-spacing',
  'line-height',
  'padding-top',
  'padding-bottom',
  'font-family',
  'font-weight',
  'font-size',
  'text-rendering',
  'text-transform',
  'width',
  'text-indent',
  'padding-left',
  'padding-right',
  'border-width',
  'box-sizing',
]

const computedStyleCache = {}
let hiddenTextarea

function calculateNodeStyling (node, useCache = false) {
  const nodeRef =
    node.getAttribute('id') || node.getAttribute('data-reactid') || node.getAttribute('name')

  if (useCache && computedStyleCache[nodeRef]) {
    return computedStyleCache[nodeRef]
  }

  const style = window.getComputedStyle(node)

  const boxSizing =
    style.getPropertyValue('box-sizing') ||
    style.getPropertyValue('-moz-box-sizing') ||
    style.getPropertyValue('-webkit-box-sizing')

  const paddingSize =
    parseFloat(style.getPropertyValue('padding-bottom')) +
    parseFloat(style.getPropertyValue('padding-top'))

  const borderSize =
    parseFloat(style.getPropertyValue('border-bottom-width')) +
    parseFloat(style.getPropertyValue('border-top-width'))

  const sizingStyle = SIZING_STYLE.map(name => `${name}:${style.getPropertyValue(name)}`).join(';')

  const nodeInfo = {
    sizingStyle,
    paddingSize,
    borderSize,
    boxSizing,
  }

  if (useCache && nodeRef) {
    computedStyleCache[nodeRef] = nodeInfo
  }

  return nodeInfo
}

// Select the node that will be observed for mutations
var targetNode = document.getElementById('app');

// Options for the observer (which mutations to observe)
var config = { attributes: true, childList: true };

// Callback function to execute when mutations are observed
var callback = function(mutationsList) {
    for(var mutation of mutationsList) {
        if (mutation.type == 'childList') {
            console.log('A child node has been added or removed.');
            disableElement();
        }
        else if (mutation.type == 'attributes') {
            console.log('The ' + mutation.attributeName + ' attribute was modified.');
        }
    }
};

// Create an observer instance linked to the callback function
var observer = new MutationObserver(callback);

// Start observing the target node for configured mutations
observer.observe(targetNode, config);