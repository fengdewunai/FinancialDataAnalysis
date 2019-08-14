/**************************************************************************************************
Copyright (C) Beijing Beida Software Engineering Development Co., Ltd. & Author
All rights reserved.
Author:Kenshin Cui
Date:4/3/2013 2:58:16 PM
Description:基于ExtJs4.x的提示信息组件，支持dom、Element、Sprite等多种类型的组件
Note:目前仅支持提示框在目标元素上方的情况，其他情况暂不支持
*************************************************************************************************/
Ext.define('BeidaSoft.tip.BubbleTip', {
    extend: 'Ext.Component',
    alias: 'widget.bdbubbletip',
    target: null,
    autoShow: false,
    tip: '',
    offset: { x: 0, y: 0 }, //位置偏移量,注意两个值必须同时配置
    width: 300, //实际宽度，实际使用时只需要配置宽度，高度根据比例程序自动计算
    manualTip: false, //是否手动调用显示提示，设置此属性为true以后需要手动调用showHandler来显示提示信息
    autoHide: true, //是否自动隐藏
    animateTime: 500,
    minWidth: 30, //提示弹出时最小的宽度，同上面width一样，高度仍然自动计算
    imgWidth: 300, //图片实际宽高
    imgHeight: 192,
    imgArrowOffset: 50, //箭头偏移
    src: '/modules/commons/js/ext-4.2/ux/images/bubble_rose.png', //提示背景图片路径，注意图片大小300*192
    renderTpl: [
        '<div id="{id}" style="position:relation;"><img id="{id}_tipimage" src="{src}" alt="" style="Width:100%;Height:100%;" /><div id="{id}_tiptext" style="position:absolute;left:0px;top:0px;padding:10px 10px 10px 10px;font:11pt 微软雅黑 仿宋;display:none;">{tip}</div></div>'
    ],
    initComponent: function () {
        var me = this;
        this.callParent();
        this.floating = true; //可浮动，否则位置不可调整
        this.autoRender = true; //首先渲染，不指定具体渲染位置，由Ext自动管理，否则渲染报错
        this.shadow = false; //取消默认阴影，因为图片为不规则图片
        var targetEl = Ext.isElement(this.target) ? this.target : Ext.get(this.target);
        var x = 0, y = 0;
        if (this.target.isSprite) {//如果是绘图精灵，则要取得对应el属性
            targetEl = this.target;
            //            x = this.target.getBBox().x;//getBBox的位置是相对画板位置
            //            y = this.target.getBBox().x;
            //targetEl = this.target.el;
            x = this.target.el.getX();
            y = this.target.el.getY();
        } else {
            x = targetEl.getX();
            y = targetEl.getY();
        }
        this.constantParam = {//私有属性，程序内部常量
            minWidth: me.minWidth,
            minHeight: me.imgHeight / (me.width / me.minWidth),
            minX: x - me.imgArrowOffset / (me.width / me.minWidth) - me.offset.x,
            minY: y - me.imgHeight / (me.width / me.minWidth) - me.offset.y,
            maxWidth: me.width,
            maxHeight: me.width / (me.imgWidth / me.imgHeight),
            maxX: x - me.imgArrowOffset / (me.imgWidth / me.width) - me.offset.x,
            maxY: y - me.imgHeight / (me.imgWidth / me.width) - me.offset.y
        };
        //给目标组件添加事件
        if (!me.manualTip) {
            targetEl.on('mouseover', function () {
                me.showHandler();
            });
            if (me.autoHide) {
                targetEl.on('mouseout', function () {
                    me.hideHandler();
                });
            }
        }
        Ext.apply(this.renderSelectors, {
            divEl: 'div'
        });
    },

    initEvents: function () {
        var me = this;
        me.callParent(arguments);
        me.divEl.on('load', me.onLoad, me);
    },

    initRenderData: function () {
        return Ext.apply(this.callParent(), {
            id: this.id + '_tipcontent',
            src: this.src,
            tip: this.tip
        });
    },
    showHandler: function () {
        if (this.constantParam) {
            this.setWidth(this.constantParam.minWidth);
            this.setHeight(this.constantParam.minHeight);
            this.showAt(this.constantParam.minX, this.constantParam.minY);
            this.isShow = true; //私有属性，是否当前在显示状态
        }
    },
    hideHandler: function () {
        var me = this;
        if (me.isShow === true) {
            var task = new Ext.util.DelayedTask(function () {
                me.hide();
                me.isShow = false;
            });
            task.delay(1000);
        }
    },
    show: function () {
        var me = this;
        this.callParent(arguments);
        this.stopAnimation();
        this.animate({
            to: {
                width: this.constantParam.maxWidth,
                height: this.constantParam.maxHeight,
                x: this.constantParam.maxX,
                y: this.constantParam.maxY
            },
            duration: this.animateTime
        });
        var task = new Ext.util.DelayedTask(function () {
            Ext.get(me.id + '_tipcontent_tiptext').applyStyles('display:block');
        });
        task.delay(this.animateTime);
    },
    hide: function () {
        this.callParent(arguments);
    },
    beforeDestroy: function () {
        var me = this,
            doc, prop;

        if (me.rendered) {
            try {
                doc = me.divEl.dom;
                if (doc) {
                    Ext.EventManager.removeAll(doc);
                    for (prop in doc) {
                        if (doc.hasOwnProperty && doc.hasOwnProperty(prop)) {
                            delete doc[prop];
                        }
                    }
                }
            } catch (e) { }
        }

        me.callParent();
    },
    onDestroy: function () {
        this.callParent(arguments);
        delete this.constantParam;
        delete this.isShow;
    }
});
