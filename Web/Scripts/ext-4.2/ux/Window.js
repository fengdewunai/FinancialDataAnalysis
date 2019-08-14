/**************************************************************************************************
Copyright (C) Beijing Beida Software Engineering Development Co., Ltd. & Author
All rights reserved.
Author:Kenshin Cui
Date:3/25/2013 11:33:05 AM
Description:基于Ext4.x的Window扩展组件
*************************************************************************************************/
Ext.define('BeidaSoft.window.Window', {
    extend: 'Ext.window.Window',
    alias: 'widget.bdwindow',
    alternateClassName: 'BeidaSoft.Window',
    animateTarget: null, //动画起始及回归目标
    width: 600,
    height: 400,
    layout: 'fit',
    //plain:true,
    modal: true,
    closeAction: 'hide',
    buttonAlign: 'center',
    params: [], //临时存储参数
    initComponent: function () {
        this.callParent(arguments);
    },
    onDestory: function () {
        this.callParent(arguments);
        delete this.params;
    },
//    afterShow: function (animateTarget, cb, scope) {
//        var me = this,
//            fromBox,
//            toBox,
//            ghostPanel;

//        if (!this.animateTarget) {
//            this.callParent(arguments);
//            //animateTarget = this.getBubbleTarget();
//        } else {
//            animateTarget = this.animateTarget;
//        }
//        //animateTarget = me.getAnimateTarget(animateTarget);

//        if (!me.ghost) {
//            animateTarget = null;
//        }
//        if (animateTarget) {
//            toBox = me.el.getBox();
//            fromBox = animateTarget.getBox();
//            me.el.addCls(me.offsetsCls);
//            ghostPanel = me.ghost();
//            ghostPanel.el.stopAnimation();

//            ghostPanel.el.setX(-10000);

//            me.ghostBox = toBox;
//            ghostPanel.el.animate({
//                from: fromBox,
//                to: toBox,
//                duration: 500,
//                listeners: {
//                    afteranimate: function () {
//                        delete ghostPanel.componentLayout.lastComponentSize;
//                        me.unghost();
//                        delete me.ghostBox;
//                        me.el.removeCls(me.offsetsCls);
//                        me.onShowComplete(cb, scope);
//                    }
//                }
//            });
//        }
//        else {
//            me.onShowComplete(cb, scope);
//        }
//    },
//    onHide: function (animateTarget, cb, scope) {
//        var me = this,
//            ghostPanel,
//            toBox,
//            activeEl = Ext.Element.getActiveElement();
//        if (!this.animateTarget) {
//            //animateTarget = this.getBubbleTarget();
//            this.callParent(arguments);
//        } else {
//            animateTarget = this.animateTarget;
//        }

//        if (activeEl === me.el || me.el.contains(activeEl)) {
//            activeEl.blur();
//        }

//        //animateTarget = me.getAnimateTarget(animateTarget);

//        if (!me.ghost) {
//            animateTarget = null;
//        }
//        if (animateTarget) {
//            ghostPanel = me.ghost();
//            ghostPanel.el.stopAnimation();
//            toBox = animateTarget.getBox();
//            ghostPanel.el.animate({
//                to: toBox,
//                duration: 500,
//                listeners: {
//                    afteranimate: function () {
//                        delete ghostPanel.componentLayout.lastComponentSize;
//                        ghostPanel.el.hide();
//                        me.afterHide(cb, scope);
//                    }
//                }
//            });
//        }
//        me.el.hide();
//        if (!animateTarget) {
//            me.afterHide(cb, scope);
//        }
//    },
    close: function () {
        if (this.closeAction == 'hide') {
            this.hide();
        } else {
            this.close();
        }
    }
});