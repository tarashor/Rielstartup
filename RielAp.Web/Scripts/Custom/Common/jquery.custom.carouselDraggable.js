/// <reference path="modernizr-2.5.3.js" />
/// <reference path="jquery-ui-1.10.0.min.js" />
(function ($) {
    $.widget("custom.carouselDraggable", {
        options: {
            isHorizontal: true,
            selectedItem: null,
            scrollSpeed: 3,
            logElement: null, 
            selectedItemClass: '',
            isDraggable: false,
            itemsContainer:null
        },

        _isScrollingLeft: false,
        _isScrollingRight: false,
        _minOffset: 0,
        _maxOffset: 0,
        _sortableWidget: null,
        _isItemMoving: false,
        _clipContainer:null,

        _cssStyles: {
            mainContainer: 'custom-carouselDraggable-mainContainer',
            leftScroll: 'custom-carouselDraggable-leftScroll',
            rightScroll: 'custom-carouselDraggable-rightScroll',
            clipper: 'custom-carouselDraggable-clipper',
            item: 'custom-carouselDraggable-item',
            basic: 'custom-carouselDraggable-basic',
            arrowImage: 'custom-carouselDraggable-scrollArrowImage',
            leftArrow:'custom-carouselDraggable-leftArrow',
            rightArrow:'custom-carouselDraggable-rightArrow'
        },

        _create: function () {
            var self = this;

            //build dom tree and apply styles
            var mainContainer = $('<div></div>').addClass(self._cssStyles.mainContainer).insertAfter(self.element);
            var leftScroll = $('<div></div>').addClass(self._cssStyles.leftScroll).appendTo(mainContainer);
            var leftArrow = $('<div></div>').addClass(self._cssStyles.arrowImage);
            leftArrow.addClass(self._cssStyles.leftArrow);
            leftScroll.append(leftArrow);
            self._clipContainer = $('<div></div>').addClass(self._cssStyles.clipper).insertAfter(leftScroll);
            var rightScroll = $('<div></div>').addClass(self._cssStyles.rightScroll).insertAfter(self._clipContainer);
            var rightArrow = $('<div></div>').addClass(self._cssStyles.arrowImage);
            rightArrow.addClass(self._cssStyles.rightArrow);
            rightScroll.append(rightArrow);
            self._clipContainer.append(self.element);

            self.element.addClass(self._cssStyles.basic);
            //self.element.css('margin-top', (-1) * self._clipContainer.height() / 2);

            //
            var itemsContainer = self.element;
            if (self.options.itemsContainer) {
                itemsContainer = self.options.itemsContainer;
            }

            itemsContainer.children().each(function (index, element) {
                $(element).addClass(self._cssStyles.item);
            });

            $('.' + self._cssStyles.item).on("click", function (e) {
                if ($('#' + self.options.nonsortableItemId).get(0) != this) {
                    self._clickItemHandler($(this), e);
                }
            });


            if (self.options.itemsContainer) {
                //set items container width
                var wic = 0;
                self.options.itemsContainer.children().each(function (index, element) {
                    wic += $(element).outerWidth(true);
                });
                self.options.itemsContainer.width(wic);
            }
            //set general width
            var wh = 0;
            self.element.children().each(function (index, element) {
                wh += $(element).outerWidth(true);
            });
            self.element.width(wh);

            this._minOffset = self._clipContainer.width() - wh;


            // bind events on elements:
            self._on(leftScroll, {
                mousedown: "_startScrollLeft",
                mouseup: "_stopScrollLeft",
                touchstart: "_startScrollLeftTouch",
                touchend: "_stopScrollLeftTouch"//,
                //touchmove: "_startMove"
            });

            self._on(rightScroll, {
                mousedown: "_startScrollRight",
                mouseup: "_stopScrollRight",
                touchstart: "_startScrollRightTouch",
                touchend: "_stopScrollRightTouch",
            });

            if (this.options.isDraggable)
            {
                this._makeDraggable(leftScroll, rightScroll);
            }
            

        },

        _setOption: function (key, value) {
            var oldValue = this.options[key];
            if (key === "selectedItem") {
                this._selectItem(value, true);
            }
            if (key === "isDraggable") {
                if (value) {
                    this._makeDraggable();
                }
                else {
                    this._deleteDraggable();
                }
            }
            if (key === "selectedItemClass") {
                if (this.options.selectedItem) {
                    $(this.options.selectedItem).removeClass(oldValue);
                    $(this.options.selectedItem).addClass(value);
                }
            }
            this._super(key, value);
        },

        _setOptions: function (options) {
            this._super(options);
        },


        _positionBeforeMoving:null,

        _makeDraggable: function (leftScroll, rightScroll) {
            var self = this;
            var itemsContainer = self.element;
            if (self.options.itemsContainer) {
                itemsContainer = self.options.itemsContainer;
            }
            self._sortableWidget = $(itemsContainer).sortable({
                scroll: true,
                axis: "x",
                start: function (event, ui) {
                    self._isItemMoving = true;
                    self._positionBeforeMoving = ui.item.parent().children().index(ui.item);
                },
                stop: function (event, ui) {
                    self._isItemMoving = false;
                    var positionAfterMoving = ui.item.parent().children().index(ui.item);
                    if (self._positionBeforeMoving != positionAfterMoving)
                    {
                        self._trigger("moved", event, { element: ui.item, newPosition: positionAfterMoving });
                    }
                },
                cancel:'#' + self.options.nonsortableItemId
            });
            leftScroll.on('mouseover', function () {
                //self._log('scroll left');
                if (self._isItemMoving) {
                    
                    self._startScrollLeft();
                }
            });
            
            rightScroll.on('mouseover', function () {
                //self._log('scroll right');
                if (self._isItemMoving) {
                    
                    self._startScrollRight();
                }
            });

            leftScroll.on('mouseout', function () {
                self._stopScrollLeft();
            });
            rightScroll.on('mouseout', function () {
                self._stopScrollRight();
            });
        },
        _deleteDraggable: function () {
            if (this._sortableWidget)
            {
                this._sortableWidget.destroy();
            }
        },

        removeSelection: function () {
            if (this.options.selectedItem) {
                $(this.options.selectedItem).removeClass(this.options.selectedItemClass);
            }
            this.options.selectedItem = null;
        },

        _clickItemHandler:function(element, e){
            var self = this;
            //self._clickItem(element, e);
            self._selectItem(element, false, e);
            self._clickItem(element, e);
        },

        _clickItem: function (element, event) {
            $(".prediction_container").hide();
            this._trigger("click", event, { element: element });
        },

        _selectItem: function (element, doNotRefreshOption, event) {
            if ($(element).get(0) != $('#' + this.options.nonsortableItemId).get(0)) {
                if ($(this.options.selectedItem).get(0) != $(element).get(0)) {
                    if (this.options.selectedItem) {
                        $(this.options.selectedItem).removeClass(this.options.selectedItemClass);
                    }
                    $(element).addClass(this.options.selectedItemClass);
                    this.scrollToItem(element);
                    if (!doNotRefreshOption)
                        this.options.selectedItem = element;

                    this._trigger("selectionChanged", event, { element: element });
                }
            }
        },

        _log: function (log) {
            if (this.options.logElement) {
                $(this.options.logElement).html($(this.options.logElement).html() + log);
            }
        },

        _startScrollRight: function () {
            var self = this;
            if (!self._isScrollingLeft && !self._isScrollingRight) {
                self._isScrollingRight = true;
                var scrollTimer = setInterval(function () {
                    if (self._isScrollingRight) {
                        self.scrollRight();
                    }
                    else {
                        clearInterval(scrollTimer);
                    }
                }, 10);
            }
        },
        _stopScrollRight: function () {
            var self = this;
            self._isScrollingRight = false;
        },

        _startScrollLeft: function () {
            var self = this;
            if (!self._isScrollingLeft && !self._isScrollingRight) {
                self._isScrollingLeft = true;
                var scrollTimer = setInterval(function () {
                    if (self._isScrollingLeft) self.scrollLeft();
                    else clearInterval(scrollTimer);
                }, 10);
            }
        },
        _stopScrollLeft: function () {
            var self = this;
            self._isScrollingLeft = false;
        },

        _startScrollRightTouch: function (e) {
            var self = this;
            e.preventDefault();
            self._startScrollRight();

        },
        _stopScrollRightTouch: function (e) {
            var self = this;
            e.preventDefault();
            self._stopScrollRight();
        },

        _startScrollLeftTouch: function (e) {
            var self = this;
            e.preventDefault();
            self._startScrollLeft();
        },
        _stopScrollLeftTouch: function (e) {
            var self = this;
            e.preventDefault();
            self._stopScrollLeft();
        },

        scrollLeft: function () {
            this.scroll(this.options.scrollSpeed);
        },

        scrollRight: function () {
            this.scroll(-this.options.scrollSpeed);
        },

        _scrollToPosition: function (position) {
            if ((position >= this._minOffset) && (position <= this._maxOffset)) {
                this.element.css('left', position + 'px');
            }
        },

        scrollToItem: function (item) {
            var self = this;
            var itemLeftOffset = item.position().left;
            var itemRightOffset = itemLeftOffset + item.outerWidth(true);// item.position().left;
            var containerWidth = self._clipContainer.width();
            var containerLeftOffset = -self.element.position().left;
            var containerRightOffset = containerLeftOffset + containerWidth;

            var position;
            if (itemLeftOffset < containerLeftOffset) {
                position = -itemLeftOffset;
            }

            if (itemRightOffset > containerRightOffset) {
                position = -(itemRightOffset - containerWidth);
            }
            this._scrollToPosition(position);
        },

        scroll: function (delta) {
            var leftOffset = this.element.position().left + delta;
            this._scrollToPosition(leftOffset);
        },

        addItemToBegin: function (item) {
            var self = this;
            var itemsContainer = self.element;
            if (self.options.itemsContainer) {
                itemsContainer = self.options.itemsContainer;
            }
            itemsContainer.prepend($(item));

            self._recalculateOnItemAdded(item);
        },

       
        addItem: function (item, itemAfter) {
            var self = this;
            if ((!!itemAfter) && (!(itemAfter.hasOwnProperty('length') && !(itemAfter.length > 0)))) {
                $(item).insertAfter(itemAfter);
            } else {
                var itemsContainer = self.element;
                if (self.options.itemsContainer) {
                    itemsContainer = self.options.itemsContainer;
                }
                itemsContainer.append($(item));
            }

            self._recalculateOnItemAdded(item);

        },

        _recalculateOnItemAdded: function (item) {
            var self = this;
            var itemWidth = $(item).outerWidth(true);

            if (self.options.itemsContainer) {
                //set items container width
                var wic = self.options.itemsContainer.outerWidth(true);
                wic += itemWidth;
                self.options.itemsContainer.width(wic);
            }

            var width = self.element.outerWidth(true);
            width += itemWidth;
            self.element.width(width);

            self._minOffset = self._clipContainer.outerWidth(true) - width;

            $(item).addClass(self._cssStyles.item);

            $(item).on("click", function (e) {
                self._clickItemHandler($(item), e);
            });
        },

        removeItem: function (item) {
            var self = this;
            var unknownSelection = false;

            var itemWidth = $(item).outerWidth(true);
            if (self.options.itemsContainer) {
                //set items container width
                var wic = self.options.itemsContainer.width();
                wic -= itemWidth;
                self.options.itemsContainer.width(wic);
            }

            var width = self.element.width();
            width -= itemWidth;
            self.element.width(width);

            self._minOffset = self._clipContainer.width() - width;

            var leftOffset = self.element.position().left;
            if (leftOffset < self._minOffset) {
                self._scrollToPosition(self._minOffset);
            }
            else {
                if (leftOffset > self._maxOffset) {
                    self._scrollToPosition(self._maxOffset);
                }
            }
            if ($(self.options.selectedItem).get(0) == $(item).get(0)) {
                var prevSibling = $(item).prev('.' + self._cssStyles.item);
                if (prevSibling.length>0) {
                    self._selectItem(prevSibling);
                }
                else {
                    unknownSelection = true;
                }
            }

            $(item).remove();
            return unknownSelection;
        }
    });
})(jQuery);